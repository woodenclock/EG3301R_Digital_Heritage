using System;
using System.Collections.Generic;
using SerialPort = System.IO.Ports.SerialPort;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class TextSwitcher : MonoBehaviour {
    public TextMeshProUGUI[] texts;                             // Array to hold all TEXT elements
    public Button nextButton;                                   // Button to go to the next text
    public Button backButton;                                   // Button to go back to the previous text
    public Button[] buttons;                                    // Array to hold all BUTTON elements
    public GameObject chosenIngredientsPanel;                   // Reference to the paper note Panel
    public TextMeshProUGUI chosenIngredientsText;               // Reference to the paper note Text

    private int scene = 0;                                      // Keeps track of which scene currently on display
    private List<string> chosenIngredientList = new();
    private SerialPort serialPort;                              // Communication with Arduino


    void Start() {                                              // Start is called before the first frame update
        try {
            serialPort = new SerialPort("COM5", 9600);          // Adjust COM port and baud rate as necessary
            serialPort.Open();
            Debug.Log("Serial port opened successfully.");
        }
        catch (Exception e) {
            Debug.LogError("Error opening serial port: " + e.Message);
        }

        for (int i = 0; i < texts.Length; i++) {                // Hide all texts
            texts[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < buttons.Length; i++) {              // Hide all buttons
            buttons[i].gameObject.SetActive(false);
        }

        texts[0].gameObject.SetActive(true);                    // Show the first text
        nextButton.gameObject.SetActive(true);                  // Enable "Next" button
        backButton.gameObject.SetActive(false);                 // Disable "Back" button on the first text
        chosenIngredientsPanel.SetActive(false);                // Hide paper note Panel
        chosenIngredientsText.gameObject.SetActive(false);      // Hide paper note Text

        nextButton.onClick.AddListener(OnNextButtonClick);      // Add listeners to buttons
        backButton.onClick.AddListener(OnBackButtonClick);
        buttons[7].onClick.AddListener(OnNextButtonClick);      // Mix! button
        buttons[8].onClick.AddListener(OnSmellButtonClick);     // Smell button

        for (int i = 3; i < 7; i++) {                           // Assign listeners dynamically to each button in the array
            int index = i;                                      // Capture index for use in the lambda
            buttons[i].onClick.AddListener(() => OnIngredientClick(index));
        }

        // Debug
        Debug.Log("STARTING SIMULATOR: " + "Scene: " + scene);
    }


    void OnSmellButtonClick() {
        if (serialPort != null && serialPort.IsOpen) {
            foreach (string ingredient in chosenIngredientList) {
                string command = "";
                // Determine which command to send based on the ingredient
                switch (ingredient) {
                    case "Laksa Leaf":
                        command = "6";  // Command for pin 5
                        break;
                    case "Lemongrass":
                        command = "7";  // Command for pin 6
                        break;
                    case "Kaffir Lime":
                        command = "8";  // Command for pin 9
                        break;
                    case "Dried Shrimp":
                        command = "9";  // Command for pin 10
                        break;
                    default:
                        Debug.LogError("Unknown ingredient: " + ingredient);
                        continue;  // Skip to the next ingredient if unknown
                }
                // Send the command to Arduino
                serialPort.WriteLine(command);
                Debug.Log("Sent command: " + command + " for ingredient: " + ingredient);
            }
        }
        else {
            Debug.LogError("Serial port not open.");
        }
    }


    void OnApplicationQuit() {
        if (serialPort != null && serialPort.IsOpen) {
            serialPort.Close();                                         // Close the serial port when the application quits
        }
    }


    private string GetIngredientName(int index) {                       // Helper function to get the ingredient name based on button index
        switch (index) {
            case 3: return "Laksa Leaf";
            case 4: return "Lemongrass";
            case 5: return "Kaffir Lime";
            case 6: return "Dried Shrimp";
            default: return "Unknown";
        }
    }


    private void UpdatePaperNote() {
        string noteContent = "Chosen Ingredients:\n";                  // Create a formatted string of chosen ingredients
        foreach (string ingredient in chosenIngredientList) {
            noteContent += "- " + ingredient + "\n";
        }
        chosenIngredientsText.text = noteContent;                      // Update the text on the paper note
    }


    // This function is called when an ingredient is clicked
    public void OnIngredientClick(int buttonIndex) {        
        if (scene == 7) {
            string chosenIngredient = GetIngredientName(buttonIndex);

            if (chosenIngredientList.Contains(chosenIngredient)) {
                chosenIngredientList.Remove(chosenIngredient);          // Remove the ingredient from the list
                UpdatePaperNote();                                      // Update the note
            } else {                                                    // If the ingredient is not in the list, add it
                chosenIngredientList.Add(chosenIngredient);             // Add the ingredient to the list
                UpdatePaperNote();                                      // Update the note
            }

            ButtonChanger();
        }
    }


    // This function is called when the "Next" button is clicked
    public void OnNextButtonClick() {
        if (scene < texts.Length - 1) {
            TextChanger(1);
            ButtonInitializer();
            ButtonChanger();
            showChosenIngredients();

            if (scene == texts.Length - 1) {                                // Disable the "Next" button if we're on the last text
                nextButton.gameObject.SetActive(false);
            }

            if (scene is 3 or 4 or 5 or 6) {                                // Show ingredient label
                buttons[scene].gameObject.SetActive(true);
            }
        }

        // Debug
        Debug.Log("Next to Scene: " + scene);
    }


    // This function is called when the "Back" button is clicked
    public void OnBackButtonClick() {
        if (scene > 0) {
            TextChanger(0);
            ButtonInitializer();
            ButtonChanger();
            showChosenIngredients();

            if (scene == 0) {                                // Disable "Back" button if we're on the first text
                backButton.gameObject.SetActive(false);
            }

            if (scene is 3 or 4 or 5) {                             // Hide ingredient label
                buttons[scene + 1].gameObject.SetActive(false);
            }
        }

        // Debug
        Debug.Log("Back to Scene: " + scene);
    }


    public void TextChanger(int number) {
        if (number > 0) {                                // Text change for "Next" button
            texts[scene].gameObject.SetActive(false);    // Hide current text
            scene++;                                     // Move to the next text
            texts[scene].gameObject.SetActive(true);     // Show the next text
        } 
        else {                                           // Text change for "Back" button
            texts[scene].gameObject.SetActive(false);    // Hide current text
            scene--;                                     // Move to the previous text
            texts[scene].gameObject.SetActive(true);     // Show the previous text
        }
    }


    public void ButtonInitializer() {
        nextButton.gameObject.SetActive(true);                  // Enable "Next" button since not last text
        backButton.gameObject.SetActive(true);                  // Enable "Back" button 
        buttons[1].gameObject.SetActive(false);                 // Disable "Start!" button
        buttons[2].gameObject.SetActive(false);                 // Disable "Back!" button
        buttons[7].gameObject.SetActive(false);                 // Disable "Mix!" button
        buttons[8].gameObject.SetActive(false);                 // Disable "Smell" button

    }


    public void ButtonChanger() {
        if (scene == 2) {
            nextButton.gameObject.SetActive(false);
            buttons[1].gameObject.SetActive(true);              // "Start!" instead of "Next" for Index == 2
            buttons[3].gameObject.SetActive(false);             // Disable "Laksa Leaves" button
        }

        if (scene == 3) {                                       // Show "StartBack" button
            backButton.gameObject.SetActive(false);
            buttons[2].gameObject.SetActive(true);
        }

        if (scene == 7) {
            nextButton.gameObject.SetActive(false);
            if (chosenIngredientList.Count > 0) {
                buttons[7].gameObject.SetActive(true);         // Show "Mix!" button
            } else {
                buttons[7].gameObject.SetActive(false);        // Hide "Mix!" button 
            }
        }
            
        if (scene == 8) {
            nextButton.gameObject.SetActive(false);
            buttons[8].gameObject.SetActive(true);              // Show "Smell" button
        }
    }

    public void showChosenIngredients() {
        if (scene is 7 or 8) {
            if (!chosenIngredientsText.gameObject.activeInHierarchy) {  // Show the paper note if not already visible
                chosenIngredientsText.gameObject.SetActive(true);
                chosenIngredientsPanel.gameObject.SetActive(true);
            }
        } else {
            chosenIngredientsText.gameObject.SetActive(false);
            chosenIngredientsPanel.gameObject.SetActive(false);
        }
    }
}