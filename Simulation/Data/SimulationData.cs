using System;
using System.Collections.Generic;
using UnityEngine;

public class SimulationData
{
  public List<List<IndividualData>> populationData;
  public List<List<Plant>> plantsData;

  public int generation;
 
  // Best Fitnesses 
  public float bestWolfFitness;
  public float bestSheepFitness;
  
  // Best Genes
  public float bestWolfSpeed;
  public float bestWolfPerception;
  public float bestWolfSize;
  public float bestSheepSpeed;
  public float bestSheepPerception;
  public float bestSheepSize;

  public SimulationData() {}

  public void initialize(List<List<Individual>> population, List<Plant> plants) {
    populationData = new List<List<IndividualData>>();
    plantsData = new List<List<Plant>>();
    for (int i = 0; i < population.Count; i++) {
      for (int j = 0; j < population[i].Count; j++) {
        populationData.Add(new List<IndividualData>());
      }
    }
    for (int i = 0; i < plants.Count; i++) {
      plantsData.Add(new List<Plant>());
    }
  }

  public void addIndividualData(List<List<Individual>> population)
  {
    for (int i = 0; i < population.Count; i++) {
      for (int j = 0; j < population[i].Count; j++) {
        populationData[i * population[0].Count + j].Add(new IndividualData(population[i][j], i));
      }
    }
  }

  public void addPlantData(List<Plant> plants)
  {
    for (int i = 0; i < plants.Count; i++) {
      plantsData[i].Add(new Plant(plants[i]));
    }
  }
}


  // POPULATION per turn:
  // [
  //   [w1, w2, w3, w4]   
  //   [s1, s2, s3]
  // ]

  // POPULATION_DATA: 
  // [
  //   w1 : [t1, t2, t3, ...] 0 * 4 + 0 = 0
  //   w2 : [t1, t2, t3, ...] 0 * 4 + 1 = 1
  //   w3 : [t1, t2, t3, ...] 0 * 4 + 2 = 2
  //   w4 : [t1, t2, t3, ...] 0 * 4 + 3 = 3
  //   s1 : [t1, t2, t3, ...] 1 * 4 + 0 = 4
  //   s2 : [t1, t2, t3, ...] 1 * 4 + 1 = 5
  //   s3 : [t1, t2, t3, ...]
  // ]

  // PLANT per turn:
  // [p1t1, p2t1, p3t1, ...]

  // PLANT_DATA: 
  // [
  //   p1 : [t1, t2, t3, ...]
  //   p2 : [t1, t2, t3, ...]
  //   p3 : [t1, t2, t3, ...]
  // ]