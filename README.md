# Performance of A* and Theta* algorithms

# Introduction
In most of the games correct AI behavior is crutial. In this project, I evaluate and compare two pathfinding algorithms to find which one is better in this scenario.

# A*
A* is one of the most commonly used pathfinding algorithms. It gives you the shortest path from a point to a different point in the world by using value "f" which is equal to the sum of value "g" where "g" is the movement cost from the starting node to another node, and value "h" which is the movement cost from the current node to the end node.
In the upcoming section I will compare two different distance calculation, Manhattan distance and Euclidean distance.

### Manhattan distance 
Manhattan distance is the sum of absolute values of the difference in the coordinates between the current node and the last node : h = abs(currentNode.x - endNode.x) + abs(currentNode.y - endNode.y). This one is used when 4 directional movement is allowed.

### Euclidean distance 
Euclidean distance is the distance between the current node and the end node using the Pithagoras theorem.
h =  Mathf.Sqrt(Mathf.Pow((currentNode.x - endNode.x), 2) + Mathf.Pow((currentNode.y - endNode.y), 2)); and it's used to calculate distance where 8 directional movements are allowed.

### Difference between Manhattan and Euclidean
In my implementation, I use both distance calculation to compare them and see which one is better and faster in this situation.
<div align="center">
  <figure>
    <img width="1575" height="880" alt="image" src="https://github.com/user-attachments/assets/d02f4906-f477-4ce9-a214-37c1b64a0b5d" />
    <figcaption><em>Figure 1:</em> <em>Note:</em> The snowmans are the agents. There are multiple agents, they just have the same position.</figcaption>
  </figure>
</div>




# AdvancedTools
<div align="center">
  <figure>
    <img width="481" height="288" alt="image" src="https://github.com/user-attachments/assets/5f12d696-cd2d-4346-8b8c-085cd962f937" />
      <br>
        <figcaption><em>Figure 2:</em> Chart comparing path length across agents.</figcaption>
      </br>
  </figure>
</div>

<div align="center">
  <figure>
    <img width="479" height="287" alt="image" src="https://github.com/user-attachments/assets/8fbc8709-99c4-49c8-ab74-c6f8af650d31" />
      <br>
        <figcaption><em>Figure 3:</em> Computation time results for A* vs Theta*.</figcaption>
      </br>
  </figure>
</div>

<div align="center">
  <figure>
    <img width="547" height="286" alt="image" src="https://github.com/user-attachments/assets/765c2c6d-50bc-4943-a51f-ecad5650528a" />
      <br>
        <figcaption><em>Figure 4:</em> Movement time comparison between algorithms.</figcaption>
      </br>
  </figure>
</div>

<div align="center">
  <figure>
    <img width="480" height="287" alt="image" src="https://github.com/user-attachments/assets/75fc09f0-5d50-4405-9f13-f098064a46ea" />
     <br>
       <figcaption>Average speed results for agents.</figcaption>
    </br>
  </figure>
</div>
