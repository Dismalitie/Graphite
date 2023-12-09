<div align="center">
  
  ![graphite](https://github.com/Dismalitie/Graphite/assets/118924562/66f4a364-4a02-401e-938c-639461e161fc)


  # Graphite

  Graphite is an open source, basic package management tool that lets you get, create and remove web packages.
</div>

> [!IMPORTANT]
> Graphite is under development, stable builds will be released in the [releases tab](https://github.com/Dismalitie/Graphite/releases).

# Getting a static package

```
graphite get [ path ] [ destination ]
```

An example path: `https://raw.githubusercontent.com/Dismalitie/JaxCore-CS/main/JaxCore-Installer/bin/Debug/graphite.gih`

Downloads all files included in a `.gih` - Graphite Installation Hive - to `destination`.

# Removing a static package

To quickly remove a package that is installed in the current directory, run:

```
graphite del [ path ] [ destination ]
```

This will remove any files associated with the package provided with the `path` argument in the `destination` directory.

# Creating a static package

To create / index a package, run:

```
graphite index [ path ] [ destination ]
```

This will add all files at `path` to an index file outputted at `destination`, which can be used when retrieving the package with [`get`](https://github.com/Dismalitie/Graphite/edit/main/README.md#gettin-a-static-package).

**Note: the `graphite` file must be put in the same directory as the files when hosting**

# Creating a dynamic package

Dynamic packages (`.npkg`) are files that will always extract the latest version. **These require internet to extract.**

To create one, run:

```
graphite netpack [ path ] [ packname ] [ destination ]
```
Replace `path` with a web address that contains a Graphite Installation Hive - see [Creating a static package](https://github.com/Dismalitie/Graphite/edit/main/README.md#creating-a-static-package)

Replace `packname` with what you want your pack to be called. E.g: `MyPack` would become `MyPack.npkg`. The package file will be outputted at `destination`.

# Getting a dynamic package

To retrieve a package, run:

```
graphite netunpack [ path ] [ destination ] 
```

Replace `path` with the location of a local `.npkg` file.

It will extract the latest copy to `destination`.

# Deleting a dynamic package

To remove a dynamic package installed on the local system, run

```
graphite netdel [ path ] [ destination ]
```

Any files associated with the `.npkg` file will be removed in the destination directory.
