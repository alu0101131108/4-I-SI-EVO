using System.Collections;
using System.Collections.Generic;

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

    // Attributes
    public int speed;             
    public int perceptionRange;   
    public int size;
    public float initialHealth;              

    IndividualData(Individual target)
    {
      this.type = target.type;
      this.xPos = target.xPos;
      this.yPos = target.yPos;
      this.action = target.action;
      this.alive = target.alive;
      this.energy = target.energy;
      this.health = target.health;
      this.perception = target.perception;
      this.speed = target.speed;
      this.perceptionRange = target.perceptionRange;
      this.size = size;
      this.initialHealth = target.initialHealth;
    }
}