<div align="center">
  
  ![graphite](https://github.com/Dismalitie/Graphite/assets/118924562/05514801-ae04-48e6-81c3-f95e3d0f5c61)



  # Graphite Hive Installer

  Graphite is an open source, basic package management tool that lets you get, create and remove web packages.
</div>


# Getting a static package

```
graphite get [ path ]
```

An example path: `https://raw.githubusercontent.com/Dismalitie/JaxCore-CS/main/JaxCore-Installer/bin/Debug/graphite.gih`

This path leads to a raw file when Graphite appends the required file to it. **It is important that the path does not end in a `/`**

# Removing a static package

To quickly remove a package that is installed in the current directory, run:

```
graphite del [ path ]
```

This will remove any files associated with the package provided with the `path` argument.

# Creating a static package

To create / index a package, run:

```
graphite index
```

This will add all files in the current directory to an index file, which can be used when retrieving the package with [`get`](https://github.com/Dismalitie/Graphite/edit/main/README.md#get-a-package).

**Note: the `graphite` file must be put in the same directory as the files when hosting**

# Creating a dynamic package

Dynamic packages (`.npkg`) are files that will always extract the latest version. **These require internet to extract.**

To create one, run:

```
graphite netpack [ path ] [ packname ]
```
Replace `path` with a web address that contains a Graphite Installation Hive - see [Creating a static package](https://github.com/Dismalitie/Graphite/edit/main/README.md#creating-a-static-package)

Replace `packname` with what you want your pack to be called. E.g: `MyPack` would become `MyPack.npkg`.

# Getting a dynamic package

To retrieve a package, run:

```
graphite netunpack [ path ]
```

Replace `path` with the location of a local `.npkg` file.
