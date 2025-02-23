{
  description = "A very basic flake";

  inputs = {
    nixpkgs.url = "github:nixos/nixpkgs?ref=nixos-unstable";
  };

  outputs = { self, nixpkgs }: {

    # WIP
    packages.x86_64-linux.wslr-nuget =
      let
        pkgs = import nixpkgs {
          system = "x86_64-linux";
        };
        nugetPackage = pkgs.buildDotnetModule {
          name = "WSLr.Cli";
          version = "0.1";
          src = pkgs.lib.fileset.toSource {
            root = ./.;
            fileset = pkgs.lib.fileset.fromSource (pkgs.lib.sources.cleanSource ./.);
          };
          projectFile = "WSLr.Cli/WSLr.Cli.csproj";
          testProjectFile = "WSLr.Tests/WSLr.Tests.csproj";
          nugetDeps = ./deps.json; # nix build .#wslr-nuget.passthru.fetch-deps && ./result deps.json && rm ./result
          packNupkg = true;
        };
      in
        nugetPackage;

    packages.x86_64-linux.ci =
      let
        pkgs = nixpkgs.legacyPackages.x86_64-linux;
        ci-package = pkgs.writeShellApplication {
          name = "ci";
          runtimeInputs = with pkgs; [
            dotnet-sdk_8
          ];
          text = ''
            dotnet test WSLr.Tests/WSLr.Tests.csproj
            dotnet pack WSLr.Cli/WSLr.Cli.csproj -o out/
            dotnet nuget push out/*.nupkg \
              --skip-duplicate \
              --source https://api.nuget.org/v3/index.json \
              --api-key "$NUGET_API_KEY"
          '';
        };
      in
        ci-package;
  };
}
