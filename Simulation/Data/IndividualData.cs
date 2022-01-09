using System.Collections;
using System.Collections.Generic;

public class IndividualData
{
  public int type;              
  public float xPos;             
  public float yPos;              
  public int action;            
  public bool alive;            

  // Statistics
  public float energy;          
  public float health;

  // Attributes
  public float speed;             
  public float perceptionRange;   
  public int size;
  public float maxEnergy;              

  // Perception
  public List<float> toWolf;
  public List<float> toSheep;
  public List<float> toPlant;

  public IndividualData(Individual target, int type_)
  {
    type = type_;
    xPos = target.xPos;
    yPos = target.yPos;
    action = target.action;
    alive = target.alive;
    energy = target.energy;
    health = target.health;

    toWolf = new List<float> {target.toWolf[0], target.toWolf[1]};
    toSheep = new List<float> {target.toSheep[0], target.toSheep[1]};
    toPlant = new List<float> {target.toPlant[0], target.toPlant[1]};

    speed = target.speed;
    perceptionRange = target.perceptionRange;
    size = size;
    maxEnergy = target.maxEnergy;
  }
}