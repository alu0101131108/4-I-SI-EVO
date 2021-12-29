using System.Collections;
using System.Collections.Generic;

// enum Actions 
// {
//   Explore,
//   Rest,
//   Eat,
//   Flee      // Run from predator (prey only).
// }

public class IndividualData
{
    public int type;              
    public int xPos;             
    public int yPos;              
    public int action;            
    public bool alive;            

    // Statistics
    public float energy;          
    public float health;         
    public float[] perception;     

    // Capabilities
    public int speed;             
    public int perceptionRange;   
    public int size;
    public float initialHealth;              

    IndividualData(int type_, int xPos_, int yPos_, int action_, bool alive_,  
        float energy_, float health_, float[] perception_, int speed_, 
        int perceptionRange_, int size_, float initialHealth_)
    {
      this.type = type_;
      this.xPos = xPos_;
      this.yPos = yPos_;
      this.action = action_;
      this.alive = alive_;
      this.energy = energy_;
      this.health = health_;
      this.perception = perception_;
      this.speed = speed_;
      this.perceptionRange = perceptionRange_;
      this.size = size_;
      this.initialHealth = initialHealth_;
    }
}

// Single generation of N Steps and M individuals.
Array Generation = [Step0, Step1, ..., StepN];
Array Step = [IndividualData1, IndividualData2, ..., IndividualDataM];