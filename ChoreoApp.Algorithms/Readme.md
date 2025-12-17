# Algorithms Overview

This folder contains small, focused helpers used to plan dancer transitions between two sets of positions.

## TransitionPlanner

- **ComputeAssignment** builds a bijection between initial and target points.
    - Primary goal: minimize the maximum travel distance for any point (bottleneck assignment).
    - Secondary goal: among solutions with that minimum radius, minimize total energy via the Hungarian algorithm.
    - The optional `energyFunc(distance)` lets you choose the cost model (defaults to squared distance).
- Internally it performs a binary search on the sorted pairwise distances to find the smallest feasible radius, then
  feeds a masked cost matrix to the Hungarian solver.

## HungarianAlgorithm

- Implements a classic O(n³) Hungarian algorithm for square cost matrices.
- Returns a sigma vector where `sigma[row] = chosenColumn`.

## DistanceHelpers

- Utility for precomputing dense matrices of squared Euclidean distances between two equally sized point sets. Used by
  `TransitionPlanner` to avoid duplicating distance loops.

## Vector2Extensions

- Inline helpers on `Vector2` for squared and Euclidean distance computations.

## Extending or Tuning

- To emphasize fairness, keep the two-stage objective (min radius, then min energy).
- Swap in a different `energyFunc` (e.g., exponential or linear) when you want smoother or sharper penalties for long
  moves.
- For very small `n` (our current use), the polynomial runtimes of the Hungarian algorithm and the feasibility search
  remain trivial.
