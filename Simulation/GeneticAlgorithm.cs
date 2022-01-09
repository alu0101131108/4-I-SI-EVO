using System;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class Deep {
  public static T Copy<T>(T item)
  {
    BinaryFormatter formatter = new BinaryFormatter();
    MemoryStream stream = new MemoryStream();
    formatter.Serialize(stream, item);
    stream.Seek(0, SeekOrigin.Begin);
    T result = (T)formatter.Deserialize(stream);
    stream.Close();
    return result;
  }
}


public static class GeneticAlgorithm
{
	public static int generation;
  public static float mapSize;

  public static List<int> populationSize;
	public static List<List<Individual>> population;
  public static List<Plant> plants;

	public static int elitism;
	public static float mutationRate;
  public static int steps;

	public static List<float> bestFitness;
	public static List<List<float>> bestGenes;
	public static List<float> fitnessSum;

  
	public static void initialize(List<int> populationSize_, int elitism_ = 1, float mutationRate_ = 0.01f, int steps_ = 10, float mapSize_ = 50f)
	{
    mapSize = mapSize_;
    populationSize = populationSize_;
		elitism = elitism_;
		mutationRate = mutationRate_;
		steps = steps_;
		generation = 0;
    plants = new List<Plant>();
    bestFitness = new List<float> {0f, 0f};
    bestGenes = new List<List<float>> {new List<float> {0f, 0f, 0f}, new List<float> {0f, 0f, 0f}};
    fitnessSum = new List<float> {0f, 0f};
    population = new List<List<Individual>> {new List<Individual>() , new List<Individual>()};


    // Creatures and plants initialization.
    float x, y;
    for (int i = 0; i < populationSize[0]; i++) {
      x = UnityEngine.Random.Range(0f, mapSize - 1);
      y = UnityEngine.Random.Range(0f, mapSize - 1);
      population[0].Add(new Wolf(x, y));
    }
    for (int i = 0; i < populationSize[1]; i++) {
      x = UnityEngine.Random.Range(0f, mapSize - 1);
      y = UnityEngine.Random.Range(0f, mapSize - 1);
      population[1].Add(new Sheep(x, y));
    }
    for (int i = 0; i < populationSize[2]; i++) {
      x = UnityEngine.Random.Range(0f, mapSize - 1);
      y = UnityEngine.Random.Range(0f, mapSize - 1);
      plants.Add(new Plant(x, y));
    }

    Debug.Log("Wolfs: " + population[0].Count.ToString());
    Debug.Log("Sheeps: " + population[1].Count.ToString());
    Debug.Log("Plants: " + plants.Count.ToString());
	}

  // Skips a number of generations and returns last generation data.
  public static SimulationData getDataFromNextGeneration(int nextGeneration)
  {
    // Perform nextGeneration number of generations.
    Debug.Log("1");
    for (int i = 0; i < nextGeneration; i++)
    {
      Debug.Log("gen: " + i.ToString());
      newGeneration();
    }
    Debug.Log("3");
    
    // Create data object from last generation.
    SimulationData DATA = newGeneration(true);
    Debug.Log("4");
    
    // Add additional info.
    DATA.generation = generation;
    DATA.bestWolfFitness = bestFitness[0];
    DATA.bestWolfSpeed = bestGenes[0][0];
    DATA.bestWolfPerception = bestGenes[0][1];
    DATA.bestWolfSize = bestGenes[0][2];
    DATA.bestSheepFitness = bestFitness[1];
    DATA.bestSheepSpeed = bestGenes[1][0];
    DATA.bestSheepPerception = bestGenes[1][1];
    DATA.bestSheepSize = bestGenes[1][2];
    Debug.Log("5");

    return DATA; 
  }

  // Evolves current generation into the next one.
  public static SimulationData newGeneration(bool save = false)
	{ 
    // Perform simulation to evaluate each creature performance.
    SimulationData DATA = null;
    Debug.Log("a1");
    if (save) DATA = simulateRetrievingData();
    else simulate();
    Debug.Log("a2");

    List<List<Individual>> newPopulation = Deep.Copy(population);
		for (int specie = 0; specie < population.Count; specie++)
    {
      // Sort population according to fitness.
      population[specie].Sort(compareFitness);
      bestFitness[specie] = population[specie][0].fitness;
      bestGenes[specie][0] = population[specie][0].speed;
      bestGenes[specie][1] = population[specie][0].perceptionRange;
      bestGenes[specie][2] = population[specie][0].size;

      newPopulation[specie].Clear();

      // Generation of new population.
      for (int i = 0; i < populationSize[specie]; i++)
      {
        // Elitism.
        if (i < elitism && i < population[specie].Count)
        {
          population[specie][i].restart();    // If specie alive, does nothing, if dead, revives in new location.
          newPopulation[specie].Add(population[specie][i]);
        }
        // Crossover and mutation.
        else
        {
          Individual parent1 = chooseParent(specie);
          Individual parent2 = chooseParent(specie);
          Individual child = parent1.Crossover(parent2, mutationRate);
          newPopulation[specie].Add(child);
        }
      }
    }
    Debug.Log("a3");

    Debug.Log("Generación completada: " + generation);
    Debug.Log("Best wolf fitness: " + bestFitness[0]);
    Debug.Log("Best sheep fitness: " + bestFitness[1]);

		generation++;
		population = Deep.Copy(newPopulation);
    Debug.Log("se llega aqui");
    return DATA;
	}

  public static void simulate()
	{
    // Every creature has steps turns to actuate.
    for (int s = 0; s < steps; s++)
    {
      for (int specie = 0; specie < population.Count; specie++)
      {
        for (int i = 0; i < population[specie].Count; i++)
        {
          population[specie][i].actuate();
        }
      }
    }
    
    // Compute fitness.
    for (int specie = 0; specie < population.Count; specie++)
    {
      Debug.Log(specie);
      Debug.Log(fitnessSum.Count);
      fitnessSum[specie] = 0;
      for (int i = 0; i < population[specie].Count; i++)
      {
        fitnessSum[specie] += population[specie][i].CalculateFitness(steps);
      }
    }
    Debug.Log("Termina simulate");
	}

  public static SimulationData simulateRetrievingData()
  {
    SimulationData DATA = new SimulationData();
    DATA.initialize(population, plants);

    // Every creature has steps turns to actuate.
    for (int s = 0; s < steps; s++)
    {
      for (int specie = 0; specie < population.Count; specie++)
      {
        for (int i = 0; i < population[specie].Count; i++)
        {
          population[specie][i].actuate();
        }
      }
      DATA.addIndividualData(population);
      DATA.addPlantData(plants);
    }

    // Compute fitness.
    for (int specie = 0; specie < population.Count; specie++)
    {
      fitnessSum[specie] = 0;
      for (int i = 0; i < population[specie].Count; i++)
      {
        fitnessSum[specie] += population[specie][i].CalculateFitness(steps);
      }
    }

    return DATA;
  }

  // Comparation criteria for individual fitnesses, in order to sort population from max to min fitness.
  public static int compareFitness(Individual a, Individual b)
	{
		if (a.fitness > b.fitness) {
			return -1;
		} else if (a.fitness < b.fitness) {
			return 1;
		} else {
			return 0;
		}
	}

  // Chooses one possible parent to be reproduced.                                              
	public static Individual chooseParent(int specie)
	{
    Debug.Log((population[specie].Count).ToString());
    int randomIndex = UnityEngine.Random.Range(0, population[specie].Count / 4);
    Debug.Log(randomIndex);
    return population[specie][randomIndex];
  
    // float randomNumber = UnityEngine.Random.Range(0f, 1f) * fitnessSum[specie];

		// for (int i = 0; i < population[specie].Count; i++)
		// {
		// 	if (randomNumber < population[specie][i].fitness)
		// 	{
		// 		return population[specie][i];
		// 	}

		// 	randomNumber -= population[specie][i].fitness;
		// }

		// return null;
	}

  //////////////////////////////////////////////
  // Helpers for individual interactions.
  //////////////////////////////////////////////
  // Looks for the nearest creature to a certain position and then kills it.
  public static void kill(int specie, float x, float y)
  {
    if (specie == 2)  // Plant (2)
    {
      for (int i = 0; i < plants.Count; i++)
      {
        if (plants[i].xPos == x && plants[i].yPos == y)
        {
          plants[i].die();
          break;
        }
      }
    }
    else  // Sheep (1)
    {
      for (int i = 0; i < population[specie].Count; i++)
      {
        if (population[specie][i].xPos == x && population[specie][i].yPos == y)
        {
          population[specie][i].die();
          break;
        }
      }
    }

    // float distance, closestDistance = 1f;
    // int closestIndex;
    // for (int i = 0; i < population[specie].Count; i++)
    // {
    //   distance = StaticMath.getDistBetweenPoints(x, y, population[specie][i].xPos, population[specie][i].yPos);
    //   if (distance < closestDistance)
    //   {
    //     closestDistance = distance;
    //     closestIndex = i;
    //   }
    // }

    // population[specie][closestIndex].kill();
  }
  // Computes the observable creatures in the map from a certain position with a given range.
  public static List<List<float>> getPerceptionFrom(float x, float y, float perceptionRange) 
  {
    int closestWolfIndex = -1, closestSheepIndex = -1, closestPlantIndex = -1; 

    // Closest wolf.
    float closestDistance = 100000f;
    for (int i = 0; i < population[0].Count; i++) 
    {
      float distance = StaticMath.GetDistBetweenPoints(population[0][i].xPos, population[0][i].yPos, x, y);
      if (distance < closestDistance && distance <= perceptionRange && distance != 0) {
        closestWolfIndex = i;
        closestDistance = distance;
      }
    }
    // Closest sheep.
    closestDistance = 100000f;
    for (int i = 0; i < population[1].Count; i++) 
    {
      float distance = StaticMath.GetDistBetweenPoints(population[1][i].xPos, population[1][i].yPos, x, y);
      if (distance < closestDistance && distance <= perceptionRange && distance != 0) {
        closestSheepIndex = i;
        closestDistance = distance;
      }
    }
    // Closest plant.
    closestDistance = 100000f;
    for (int i = 0; i < plants.Count; i++) 
    {
      float distance = StaticMath.GetDistBetweenPoints(plants[i].xPos, plants[i].yPos, x, y);
      if (distance < closestDistance && distance <= perceptionRange) {
        closestPlantIndex = i;
        closestDistance = distance;
      }
    }

    // Check what happens when nothing in range.
    float wolfX = closestWolfIndex == -1 ? perceptionRange : population[0][closestWolfIndex].xPos - x;
    float wolfY = closestWolfIndex == -1 ? perceptionRange : population[0][closestWolfIndex].yPos - y;
    float sheepX = closestSheepIndex == -1 ? perceptionRange : population[1][closestSheepIndex].xPos - x;
    float sheepY = closestSheepIndex == -1 ? perceptionRange : population[1][closestSheepIndex].yPos - y;
    float plantX = closestPlantIndex == -1 ? perceptionRange : plants[closestPlantIndex].xPos - x;
    float plantY = closestPlantIndex == -1 ? perceptionRange : plants[closestPlantIndex].yPos - y;

    List<float> wolf = new List<float> {wolfX, wolfY};
    List<float> sheep = new List<float> {sheepX, sheepY};
    List<float> plant = new List<float> {plantX, plantY};

    return new List<List<float>> {wolf, sheep, plant};
  }
}