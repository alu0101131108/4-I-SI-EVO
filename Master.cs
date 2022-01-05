using System;
using System.Text;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class Master : MonoBehaviour
{
  public int wolves = 20;
  public int sheeps = 20;
  public int plants = 10;
  public int steps = 50;
  public int elitism = 3;
  public float mutationRate = 0.01f;

  void Start()
  {
    Interface MENU = new Interface();

    List<int> populationSizes = new List<int> {wolves, sheeps, plants};
    GeneticAlgorithm EVO = new GeneticAlgorithm(populationSizes, elitism, mutationRate, steps);

    int generations = MENU.getInputGeneration();+

    SimulationData data = EVO.getDataFromNextGeneration(generations);

    MENU.RunSimulation(data);
  }

}