using System.Collections;
using System.Collections.Generic;

public class Wolf: Individual {
  Wolf() {
    Random rand = new Random(Guid.NewGuid().GetHashCode());
    int[] speedRange = {40, 60};
    int[] perceptionRangeRange = {10, 20};
    int[] sizeRange = {5, 10};
    
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
}