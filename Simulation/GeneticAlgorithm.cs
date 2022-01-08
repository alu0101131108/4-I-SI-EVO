using System;
using System.Collections.Generic;

public class GeneticAlgorithm
{
	public int generation;

  public List<int> populationSize;
	public List<List<Individual>> population;
  public List<Plant> plants;

	public int elitism;
	public float mutationRate;
  public int steps;

	public List<float> bestFitness;
	public List<List<float>> bestGenes;
	public List<float> fitnessSum;


	public GeneticAlgorithm(List<int> populationSize_, int elitism_ = 1, float mutationRate_ = 0.01f, int steps_ = 10, int mapSize = 50)
	{
    populationSize = populationSize_;
		elitism = elitism_;
		mutationRate = mutationRate_;
		steps = steps_;
		generation = 0;

    // Creatures and plants initialization.
    Random rand = new Random(Guid.NewGuid().GetHashCode());
    float x, y;
    for (int i = 0; i < populationSize[0]; i++) {
      x = rand.NextDouble(0, mapSize - 1);
      y = rand.NextDouble(0, mapSize - 1);
      population[0].Add(new Wolf(x, y));
    }
    for (int i = 0; i < populationSize[1]; i++) {
      x = rand.NextDouble(0, mapSize - 1);
      y = rand.NextDouble(0, mapSize - 1);
      population[1].Add(new Sheep(x, y));
    }
    for (int i = 0; i < populationSize[2]; i++) {
      x = rand.NextDouble(0, mapSize - 1);
      y = rand.NextDouble(0, mapSize - 1);
      plants.Add(new Plant(x, y));
    }
	}

  // Skips a number of generations and returns last generation data.
  public SimulationData getDataFromNextGeneration(int nextGeneration)
  {
    // Perform nextGeneration number of generations.
    for (int i = 0; i < nextGeneration - 1; i++)
    {
      newGeneration();
    }

    // Create data object from last generation.
    SimulationData DATA = newGeneration(true);

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

    return DATA; 
  }

  // Evolves current generation into the next one.
	public SimulationData newGeneration(bool save = false)
	{ 
    // Perform simulation to evaluate each creature performance.
    SimulationData DATA;
    if (save) DATA = simulateRetrievingData();
    else simulate();
 
    List<List<Individual>> newPopulation = population;
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
          newPopulation[specie].Add(population[specie][i]);
          population[specie][i].restart();    // If specie alive, does nothing, if dead, revives in new location.
        }
        // Crossover and mutation.
        else
        {
          Individual parent1 = chooseParent();
          Individual parent2 = chooseParent();
          Individual child = parent1.Crossover(parent2, mutationRate);
          newPopulation[specie].Add(child);
        }
      }
    }
		generation++;
		population = newPopulation;
    return DATA;
	}
	
  // Runs the simulation of one concrete generation.
	public void simulate()
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
      fitnessSum[specie] = 0;
      for (int i = 0; i < population[specie].Count; i++)
      {
        fitnessSum[specie] += population[specie][i].calculateFitness(steps);
      }
    }
	}

  public SimulationData simulateRetrievingData()
  {
    SimulationData DATA;

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
        fitnessSum[specie] += population[specie][i].calculateFitness(steps);
      }
    }

    return DATA;
  }

  // Comparation criteria for individual fitnesses, in order to sort population from max to min fitness.
	public int compareFitness(Individual a, Individual b)
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
	public Individual chooseParent()
	{
		double randomNumber = random.NextDouble() * fitnessSum;

		for (int i = 0; i < population.Count; i++)
		{
			if (randomNumber < population[i].Fitness)
			{
				return population[i];
			}

			randomNumber -= population[i].Fitness;
		}

		return null;
	}

  //////////////////////////////////////////////
  // Static helpers for individual interactions.
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
          plants[i].kill();
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
          population[specie][i].kill();
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
      float distance = StaticMath.getDistBetweenPoints(population[0][i].xPos, population[0][i].yPos, x, y);
      if (distance < closestDistance && distance <= perceptionRange && distance != 0) {
        closestWolfIndex = i;
        closestDistance = distance;
      }
    }
    // Closest sheep.
    closestDistance = 100000f;
    for (int i = 0; i < population[1].Count; i++) 
    {
      float distance = StaticMath.getDistBetweenPoints(population[1][i].xPos, population[1][i].yPos, x, y);
      if (distance < closestDistance && distance <= perceptionRange && distance != 0) {
        closestSheepIndex = i;
        closestDistance = distance;
      }
    }
    // Closest plant.
    closestDistance = 100000f;
    for (int i = 0; i < plants.Count; i++) 
    {
      float distance = StaticMath.getDistBetweenPoints(plants[i].xPos, plants[i].yPos, x, y);
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