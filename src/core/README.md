# Using Shared LaTeX Files Across Multiple Projects with Git Submodules

If you have multiple LaTeX projects that share common settings or commands, you can manage those shared files using Git submodules. This way, you maintain a single source of truth for your shared LaTeX files, and any updates to them can be easily propagated across all your projects.

If you like to use my shared LaTeX settings and commands, you can find them in this repository. And you use them in your own projects as follows:


In your main repo:

```
latex-projecten/
├─ src/
│   ├─ main.tex
.   ├─ project-specific stuff...
.   └─ core/  ← submodule, don't create this folder yourself
```

You add it as a submodule:

```bash
git submodule add https://github.com/matttersteege/latex-core src/core
```

Then, in your LaTeX files, you include the shared ones like this:

```LaTeX
\include{core/paperSettings} % import custom general paper settings
\include{core/standardCommands} % import custom commands and packages
```

---

(Just for me as a reminder):

Whenever you update something in latex-core, you just:

```bash
cd src/core
git pull
cd ../..
git add src/core
git commit -m "Update shared settings"
```

And all branches can `git submodule update --remote` to sync those exact same files.
