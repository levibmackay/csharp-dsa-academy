# C# DSA Academy

**Status: in progress** — see [`PROGRESS.md`](PROGRESS.md) for which modules are done.

A project-based, offline-first curriculum for relearning Data Structures & Algorithms in C#. Every module is a real, runnable .NET project with a starter stub, a full xUnit test suite that defines "done," and a reference solution to fall back on.

## About this project

I had Claude scaffold the curriculum structure — the 12 module folders, the stub method signatures, the xUnit test suites that define "done" for each one, and the reference solutions. From there, the actual implementation work is mine: I'm working through each module myself, writing the code that makes the tests go green, and documenting my reasoning as I go (in commit messages and in-code comments explaining *why* an approach works, not just what it does). The scaffolding saved me the boilerplate of writing 12 sets of test suites by hand; the DSA work itself — understanding the problems, implementing the algorithms, debugging failing tests — is what I'm actually doing and learning from.

Built for working **offline** with only a local Ollama model for help — every README is self-contained: syntax refreshers, problem statements, escalating hints, and worked examples for the hard parts. You shouldn't need internet access to get through this.

## Prerequisites

- .NET SDK installed (`dotnet --version` — this was built against .NET 10; anything reasonably recent works).
- That's it. No IDE required, though VS Code / Rider / Visual Studio all work fine if you have one.

## How this works

Each module lives in `modules/NN-topic-name/` with three folders:

- **`src/<ProjectName>/`** — the starter project. Classes/methods are already declared with the exact signatures you need; every method body currently just does `throw new NotImplementedException();`. **This is the only folder you edit.**
- **`tests/<ProjectName>.Tests/`** — a full xUnit test suite. This is the spec. Your job is done when these pass.
- **`solution/Solution.cs`** — a complete, correct reference implementation. It's a plain `.cs` file, not wired into any project, so it never interferes with your build. Read it only after a real attempt — or after you pass, to compare approaches.

### The loop for every module

1. `cd modules/NN-topic-name`
2. Read `README.md` top to bottom — syntax refresher first, then problems, then hints if you need them.
3. Open `src/<ProjectName>/` and start implementing.
4. Run the tests (see below). Red means keep going, green means move on.
5. Optional: read `solution/Solution.cs` and compare your approach — different valid solutions are common, this isn't about matching it exactly.

### Running tests

From a module's test folder:

```bash
cd modules/01-arrays-strings/tests/ArraysStrings.Tests
dotnet test
```

Or run everything in the whole academy at once from the repo root:

```bash
dotnet test
```

(Expect everything to fail with `NotImplementedException` until you start implementing — that's the correct starting state, not a bug.)

## Curriculum (do these in order — each builds on the last)

| # | Module | Topic | Builds on |
|---|--------|-------|-----------|
| 01 | [arrays-strings](modules/01-arrays-strings/README.md) | Arrays & Strings | — |
| 02 | [linked-lists](modules/02-linked-lists/README.md) | Singly Linked Lists | 01 |
| 03 | [stacks-queues](modules/03-stacks-queues/README.md) | Stacks & Queues (array-backed, circular buffer) | 01 |
| 04 | [recursion-backtracking](modules/04-recursion-backtracking/README.md) | Recursion, Memoization, Backtracking | 02 |
| 05 | [hashing](modules/05-hashing/README.md) | Hash Tables (separate chaining) | 01, 04 |
| 06 | [trees-bst](modules/06-trees-bst/README.md) | Binary Search Trees | 02, 04 |
| 07 | [heaps-priority-queues](modules/07-heaps-priority-queues/README.md) | Binary Heaps / Priority Queues | 05, 06 |
| 08 | [graphs-bfs-dfs](modules/08-graphs-bfs-dfs/README.md) | Graphs — BFS & DFS | 03, 05 |
| 09 | [sorting-algorithms](modules/09-sorting-algorithms/README.md) | Sorting (bubble, insertion, merge, quick) | 01, 04 |
| 10 | [searching-algorithms](modules/10-searching-algorithms/README.md) | Binary Search & variants | 09 |
| 11 | [dynamic-programming](modules/11-dynamic-programming/README.md) | Dynamic Programming (tabulation) | 04, 09 |
| 12 | [tries-union-find](modules/12-tries-union-find/README.md) | Tries & Union-Find | 05, 06 |

Track your progress in [`PROGRESS.md`](PROGRESS.md) — check off each module as its tests go green.

## Working with a weak offline LLM

A small local Ollama model will hallucinate C# API details and confidently write wrong code. Get more out of it by controlling what you ask:

- **Ask for an approach, not code.** "What's the algorithmic idea for finding a cycle in a linked list with O(1) space?" gets a better answer than "write HasCycle for me."
- **Paste the exact method signature and constraints** from the README, not a vague restatement. Rusty models do better with precise, narrow prompts than open-ended ones.
- **Don't trust its C# syntax blindly.** If it gives you code, check it against the syntax refresher section in the module README before assuming it compiles.
- **Use it to explain a concept you already partially understand**, not to discover one from scratch — it's much more reliable as a rubber duck / explainer than as an unsupervised code generator.
- **When it's wrong, the tests will tell you.** That's the whole point of the test-first structure here — you don't have to trust the model's claim that something is correct, you have a ground truth to run against.

## Solution structure notes

- The root `CSharpDsaAcademy.slnx` includes every module's `src` and `tests` project, so `dotnet build`/`dotnet test` from the repo root touches everything.
- `solution/Solution.cs` files are intentionally **not** part of any `.csproj` — they exist purely as reference text, so there's never a naming collision with your in-progress `src` code and they never accidentally get compiled or run.

**Last updated:** 2026-07-29

