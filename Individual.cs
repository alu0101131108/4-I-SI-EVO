using System.Collections;
using System.Collections.Generic;

enum Actions 
{
  Explore,
  Rest,
  Eat,
  Flee      // Run from predator (prey only).
}

public class Individual
{
    public NeuralNetwork brain; // NN that defines behaviour by deciding actions based on inputs.

    public int xPos;            // X Coordinate.
    public int yPos;            // Y Coordinate.
    public int action;          // Current state decided by the brain.
    public bool alive;          // Living status.

    // Statistics - Brain will have these as inputs.
    public float energy;        // Movement decreases it and regenerates with time.
    public float health;        // Decreases when hungry, increases when eats food.
    public float[] perception;  // Contains distance to nearest [Predator, Prey, PreyFood].

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
    public void actuate();  // First think, then do action.
    public void explore();  // Move around.
    public void rest();     // Not moving.
    public void eat();      // Go to food and eat it.
    
    // Helpers
    public void moveTowards(int x, int y);  // Moves n units towards (x, y). N depends on velocity and energy.

    // Methods related to the Genetic Algorithm.
    public float CalculateFitness(int index);
    public Individual Crossover(Individual otherParent);
}
