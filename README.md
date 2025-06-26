# YoButton
A sleek alternative to Unity buttons with built-in effects and theming.

## Features
- **Easy to Use** – Just add the YoButton component to any Image object.
- **Customizable** – Adjust colors, effects, and animations directly from the Inspector.
- **Responsive** – Built-in feedback for hover, press, and selection states.
- **Theming Support** – Integrates easily with your custom UI themes.
- **Events** – Built-in UnityEvents for click, hover, and selection.

## Getting Started

1. **Import** the YoButton package into your Unity project.

2. **Add** the `YoButton` component to any object with an `Image`.

3. **Customize** the button through the Inspector:
   - **Graphic Target**: Assign the target image for visual updates.
   - *Leave empty to use the object's default graphic.*
     
      ![Settings](https://github.com/user-attachments/assets/c8b79911-52ea-4384-8c53-a3303735143d)

   - **Theme Type**:
     - `Default`: Uses the standard built-in style.
     - `Manager Feed`: (Coming soon)
     - `Custom`: Assign a custom `ThemeData` ScriptableObject.
     - 
      ![Theme](https://github.com/user-attachments/assets/dbdb1984-c7c7-49e6-88e1-4a25fd57f59c)

   - **Add Effects** for each interaction:
     - **Hover Effect**: Triggered on pointer enter.
     - **Click Effect**: Triggered on click.
     - **Selected Effect**: Triggered when selected.

     Each effect supports:
     - `Color Change` – Changes button color.
     - `Scale Animation` – Resizes on interaction.
     - *(More effects coming soon)*
       
      ![Effects](https://github.com/user-attachments/assets/425daa27-f650-462d-9f72-6dec68792df1)

   - **Subscribe** to the button's UnityEvents as needed.
     
      ![Events](https://github.com/user-attachments/assets/da09b2bb-0f9f-4008-9443-1462b776db59)

4. Enjoy a modern, responsive button in your Unity UI!
