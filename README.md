# Interactive Tutorial System for Unity

This repository contains a Unity package for creating interactive tutorials for your game. The system allows you to create step-by-step tutorials with conditions, enabling and disabling UI elements or game objects, and customizing the appearance and behavior of tutorial pop-ups.

## Features

- Easily create step-by-step tutorials with custom conditions
- Enable or disable UI elements or game objects during each tutorial step
- Customize the appearance and behavior of tutorial pop-ups
- Import and export tutorial data to and from JSON files
- Works with both 2D and 3D projects
- Can be used in conjunction with other game systems

## Installation

1. Download the latest release of the Interactive Tutorial System package from the [Releases](https://github.com/username/InteractiveTutorialSystem/releases) page.
2. Open your Unity project and go to `Assets` > `Import Package` > `Custom Package`.
3. Select the downloaded package and click "Import".

## Usage

1. Add the `Tutorial` script to a new or existing GameObject in your Unity scene.
2. Configure the tutorial settings in the Inspector, such as adding steps, setting conditions, and customizing the appearance of tutorial pop-ups.
3. Create a method in your game logic to start the tutorial, e.g., `tutorial.Begin()`.
4. Optionally, create methods to skip or end the tutorial, e.g., `tutorial.SkipTutorial()` and `tutorial.EndTutorial()`.

## Example

Here's a simple example of how to use the Interactive Tutorial System:

```csharp
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Tutorial tutorial;

    void Start()
    {
        if (PlayerPrefs.GetInt("tutorial_completed", 0) == 0)
        {
            tutorial.Begin();
        }
    }

    public void SkipTutorial()
    {
        tutorial.SkipTutorial();
    }

    public void EndTutorial()
    {
        tutorial.EndTutorial();
    }
}
```

In this example, the GameManager script checks whether the player has completed the tutorial using a PlayerPrefs value. If the tutorial hasn't been completed, it starts the tutorial. The GameManager also has methods for skipping or ending the tutorial.

## Contributing
If you'd like to contribute to the development of the Interactive Tutorial System for Unity, please submit a pull request with your changes or create an issue to discuss potential improvements.

# License
The Interactive Tutorial System for Unity is released under the MIT License.
