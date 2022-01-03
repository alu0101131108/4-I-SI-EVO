using System.Collections;
using System.Collections.Generic;

public class Sheep: Individual {
  Sheep() {
    Random rand = new Random(Guid.NewGuid().GetHashCode());
    int[] speedRange = {30, 40};
    int[] perceptionRangeRange = {10, 20};
    int[] sizeRange = {10, 15};
    
    speed = rand.Next(speedRange[0], speedRange[1]);
    perceptionRange = rand.Next(perceptionRangeRange[0], perceptionRangeRange[1]);
    size = rand.Next(sizeRange[0], sizeRange[1]);

    health = size * 10;
    energy = health;
    alive = true;
  }

  public void eat() {
    health += 20;
  }

  public void flee() {

  }
}