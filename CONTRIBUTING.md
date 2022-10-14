# Contributing

Contributions are very welcome. The following will provide some helpful guidelines.

## How to build the project

Magic3D requires Unity version 2020.3.14f1 or above to build.


## How to contribute

If you want to tackle an existing issue please add a comment to make sure the issue is sufficiently discussed
and that no two contributors collide by working on the same issue. 
To submit a contribution, please follow the following workflow:

* Fork the project
* Create a feature branch
* Add your contribution
* When you're completely done, build the project and run all tests via `./gradlew clean build -PallTests`
* Create a Pull Request

### Commits

Commit messages should be clear and fully elaborate the context and the reason of a change.
If your commit refers to an issue, please post-fix it with the issue number, e.g.

```
Issue: #123
```

Furthermore, commits should be signed off according to the [DCO](DCO).

### Pull Requests

If your Pull Request resolves an issue, please add a respective line to the end, like

```
Resolves #123
```

### Formatting, Styling, Naming

We recommend using Domain Driven Design (DDD) to name the classes and members.
don't use any `*` imports at any time.
Remove unused code.
Do not save code in comments. Github versioning helps with code history.

