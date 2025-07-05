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
     
      ![Settings](https://github.com/user-attachments/assets/b5756203-0c6a-45b6-b911-35bfb4bb69fe)


   - **Theme Type**:
     - `Default`: Uses the standard built-in style.
     - `Manager Feed`: (Coming soon)
     - `Custom`: Assign a custom `ThemeData` ScriptableObject.
      ![Theme](https://github.com/user-attachments/assets/71c20fe7-1c85-469d-84f0-30f2e3836abe)


   - **Add Effects** for each interaction:
     - **Hover Effect**: Triggered on pointer enter.
     - **Click Effect**: Triggered on click.
     - **Selected Effect**: Triggered when selected.

     Each effect supports:
     - `Color Change` – Changes button color.
     - `Scale Animation` – Resizes on interaction.
     - *(More effects coming soon)*
       
      ![Effects](https://github.com/user-attachments/assets/4f14a59f-97db-4b0e-87b7-f2044107d952)


   - **Subscribe** to the button's UnityEvents as needed.
     
      ![Events](https://github.com/user-attachments/assets/64dea89d-730c-45e6-9280-578c41959796)


4. Enjoy a modern, responsive button in your Unity UI!
