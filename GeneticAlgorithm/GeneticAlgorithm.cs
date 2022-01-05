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
	public List<NeuralNetwork> bestGenes; // Investigate how can genes be represented as a string. (also how to copy)
	public List<float> fitnessSum;


	public GeneticAlgorithm(List<int> populationSize_, int elitism_ = 1, float mutationRate_ = 0.01f, int steps_ = 10)
	{
    populationSize = populationSize_;
		elitism = elitism_;
		mutationRate = mutationRate_;
		steps = steps_;
		generation = 0;

    // Creatures and plants initialization.
    for (int i = 0; i < populationSize[0]; i++) {
      population[0].Add(new Wolf());
    }
    for (int i = 0; i < populationSize[1]; i++) {
      population[1].Add(new Sheep());
    }
    for (int i = 0; i < populationSize[2]; i++) {
      plants.Add(new Plant());
    }
	}

  // Skips a number of generations and returns last generation data.
  public SimulationData getDataFromNextGeneration(int nextGeneration)
  {
    // Perform nextGeneration number of generations.
    for (int i = 0; i < nextGeneration; i++)
    {
      newGeneration();
    }

    // Create data object.
    SimulationData DATA = new SimulationData();
    DATA.setPlants
    return DATA; 
  }

  // Evolves current generation into the next one.
	public void newGeneration()
	{ 
    // Perform simulation to evaluate each creature performance.
    simulate();

    List<List<Individual>> newPopulation = population;
		for (int specie = 0; specie < population.Count; specie++)
    {
      // Sort population according to fitness.
      population[specie].Sort(compareFitness);
      bestFitness[specie] = population[specie][0].fitness;
      bestGenes[specie] = population[specie][0].brain;
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

  //////////////////
  // Static helpers for individual interactions.
  //////////////////

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
    int closestWolfIndex, closestSheepIndex, closestPlantIndex; 

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

    wolf = new List<float> {population[0][closestWolfIndex].xPos - x, population[0][closestWolfIndex].yPos - y};
    sheep = new List<float> {population[1][closestSheepIndex].xPos - x, population[0][closestSheepIndex].yPos - y};
    plant = new List<float> {plants[closestPlantIndex].xPos - x, plants[closestPlantIndex].yPos - y};

    return new List<List<float>> {wolf, sheep, plant};
  }
}
