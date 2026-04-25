# Slot-Machine-Game
## Game Link

https://minerji.github.io/Slot-Spinning-Game/
## Game Overview
The game starts every player with a default balance of 100 units. The objective is to spin the reels and align symbols to win multipliers based on the bet amount.
Reel Configuration: 3 slots.
Betting Options: Players can choose between three bet levels: 10, 20, or 30 money units per spin.
Total Symbols: 5 distinct symbols.
The returns are calculated as a multiplier of the current bet:
Triple 7s: 5x Return
Triple Cherries: 4x Return
Triple Bells: 3x Return
Triple Bars: 2x Return
Star (Special): The Star is a unique symbol that provides rewards without requiring a triple match, acting as a "scatter" or "wild" bonus.

## WebGL Build Instructions
 ### Option 1: Using VS Code (Try going with this option)
This is the easiest method if you are using Visual Studio Code.
Open Folder: Launch VS Code and open the project folder containing index.html, Build, and TemplateData.
Install Extension: * Go to the Extensions view (click the square icon on the left or press Ctrl+Shift+X).
Search for "Live Server" (by Ritwick Dey).
Click Install.
Start Server: * Once installed, you will see a "Go Live" button in the bottom-right status bar of VS Code.
Click Go Live.
Play: Your default browser will automatically open to http://127.0.0.1:5500.

### Option 2: Using Python
If you have Python installed on your system, you can start a server directly from the terminal.
Open Terminal: Open your Command Prompt (Windows) or Terminal (Mac/Linux).
Navigate to Project: Use the cd command to enter your project directory:
cd path/to/your/project-folder
Start the Server:
Run python -m http.server 8000
Play: Open your browser and type localhost:8000 into the address bar.

## Bonus Feature
The "Star" Multiplier
The Star symbol acts as a "Scatter" bonus. It does not require a specific line match and pays out based on the total count visible:
1 Star: 2x payout
2 Stars: 3x payout
3 Stars: 4x payout
Real-Time Status Messaging
"Spinning...": Active during reel motion.
"Try Again": Displayed if no winning combinations are met.
"Payout: [Amount]": Displayed immediately upon a successful win.

## Technical Approach
### 1. Weighted Random Logic
To control the game's difficulty and "Return to Player" (RTP), I implemented a weighted
probability system:

The system calculates the Net Weight (sum of all symbol weights).
A random value is chosen in the range [0, Net Weight].
The value is mapped against cumulative weight ranges to determine the resulting symbol.

Scalability: This allows for easy addition of new symbols by adjusting the weight table
without changing the core randomization code.

### 2. Data-Driven Configuration
A dedicated data script stores all symbol categories, weights, and reward multipliers.
Acts as a "Single Source of Truth."
Other scripts (Spin Manager, Transaction System) read from this centralized data, making
it easy to balance the game.

## 3. Sequential Spinning System
Gradual Stop: Manages the initiation of the spin and ensures each slot stops one after
another for a realistic feel.
Evaluation: Once all reels have reached a complete stop, it triggers the Transaction
System to evaluate the results.

## 4. Transaction Management
Bet Initialization: Validates balance and deducts the selected bet (10, 20, or 30).
Credit Logic: Calculates the reward based on the final symbols and credits the amount to
the user's balance.
Starting Balance: All players begin with 100 Money.