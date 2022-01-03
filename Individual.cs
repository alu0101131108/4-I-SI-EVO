using System.Collections;
using System.Collections.Generic;

enum Actions 
{
  Explore,
  Rest,
  Chase,    // Chase a prey (predator only). This way the wolves can decide to keep chasing a prey or stop doing it (Switching to Explore or Rest)
  Flee      // Run from predator (prey only).
}

public class Individual
{
    public NeuralNetwork brain; // NN that defines behaviour by deciding actions based on inputs.

    public float xPos;          // X Coordinate.
    public float yPos;          // Y Coordinate.
    public int action;          // Current state decided by the brain.
    public bool alive;          // Living status.

    // Statistics - Brain will have these as inputs.
    public float energy;        // Movement decreases it and regenerates with time.
    public float health;        // Always decreases, increases when eats food. Healthiest individuals will succeed in natural selection.
    public float[] toPredator;  // Contains vector to nearest predator. 
    public float[] toPrey;      // Contains vector to nearest prey.
    public float[] toPlant;     // Contains vector to nearest food.

    // Capabilities - Performance of actions depend on these.
    public int speed;           // Moves faster but increase energy usage.
    public int perceptionRange; // Sees further but increase energy usage.
    public int size;            // Increases initial health but increases energy usage.

    Individual();
    
    // Brain inputs updation.
    public void updateEnergy();
    public void updateHealth();
    public void updatePerception();
    
    // Cicle simulation.
    public void explore() {           // Walks to a random position inside its range and checks for food.
      Random rand = new Random(Guid.NewGuid().GetHashCode());
      moveTowards(rand.Next(xPos - perceptionRange, xPos + perceptionRange),
                  rand.Next(yPos - perceptionRange, yPos + perceptionRange));
      eat();
    }

    public void rest() {              // Regenerates energy while not moving.
      energy++;
      health--;
    }

    public void kill() {              // Kills the animal.
      alive = false;
      health = -1;
    }

    public virtual void actuate();    // First think, then do action.
    public virtual void eat();        // Goes after food within its range if there's any.
    
    // Helpers
    public void moveTowards(int x, int y) {  // Moves n units towards (x, y). N depends on velocity and energy.
      
    }

    // Methods related to the Genetic Algorithm.
    public float CalculateFitness(int index);
    public Individual Crossover(Individual otherParent);
}
