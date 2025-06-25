# 🐢 Turtle Agent – Unity ML-Agents Project

This project demonstrates a Reinforcement Learning agent built using **Unity** and **ML-Agents Toolkit**. The agent (a turtle) learns to navigate towards a randomly placed goal using Proximal Policy Optimization (PPO).

## 🎮 Project Overview

- Developed in Unity using the ML-Agents package
- Trained a turtle (cylinder) to reach a goal (sphere) on a platform
- Used custom reward functions and observations
- Training parallelized over 12 environments for faster convergence

## 🧠 Learning Configuration

- **Algorithm**: PPO (Proximal Policy Optimization)
- **Steps**: 1,000,000
- **Mean Reward Achieved**: ~0.94
- **Observations**:
  - Turtle position, rotation
  - Goal position

## 📁 Folder Structure
<pre> Turtle-Agent/ 
  ├── Assets/
  ├── Packages/
  ├── ProjectSettings/ 
  ├── config/ 
  │ └── <a href="config/Turtle.yaml">Turtle.yaml</a> 
  ├── README.md 
  └── results/
    └── manyturtles-long
</pre>


## 🏁 Training Command

```bash
mlagents-learn config/Turtle.yaml --run-id=Turtle_run
```

## 🎥 Demo
A short video of the agent learning to reach the goal is available here:


## Requirements
- Unity 2023 or later
- ML-Agents Toolkit
- Python

## 🧠 Installation

This project uses the [Unity ML-Agents Toolkit](https://github.com/Unity-Technologies/ml-agents).


## License
This project for education purpose and free to use.
