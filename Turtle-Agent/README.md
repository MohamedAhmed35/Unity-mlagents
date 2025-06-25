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
Turtle-Agent/

├── Assets/

├── ProjectSettings/

├── config/

│ └── Turtle.yaml

├── Scripts/

│ ├── TurtleAgent.cs

│ └── GUI_TurtleAgent.cs

├── README.md
└── (Unity project files)
