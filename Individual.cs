using System.Collections;
using System.Collections.Generic;


public class Individual
{
    public NeuralNetwork brain;   // NN that defines behaviour by deciding actions based on inputs.

    public float xPos;            // X Coordinate.
    public float yPos;            // Y Coordinate.
    public int action;            // Current state decided by the brain.
    public bool alive;            // Living status.
    public float energyLossRate;  // Rate at which energy will be lowered.

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
    public void explore() {           // Walks to a random position inside its range.
      Random rand = new Random(Guid.NewGuid().GetHashCode());
      moveTowards(rand.Next(xPos - perceptionRange, xPos + perceptionRange),
                  rand.Next(yPos - perceptionRange, yPos + perceptionRange));
    }

    public void rest() {              // Regenerates energy while not moving.
      energy += 10;
    }

    public void kill() {              // Kills the animal.
      alive = false;
      health = -1;
    }

    public virtual void actuate();    // First think, then do action.
    public virtual void eat();        // Goes after food within its range if there's any.
    
    // Helpers
    public void moveTowards(int x, int y) {  // Moves n units towards (x, y). N depends on velocity and energy.
      if (energy > 0) {
        int diffX = x - xPos;
        int diffY = y - yPos;
        xPos += speed * diffX;
        yPos += speed * diffY;
        energy -= energyLossRate;
      }
    }

    // Methods related to the Genetic Algorithm.
    public float CalculateFitness(int index);
    public Individual Crossover(Individual otherParent);
}
