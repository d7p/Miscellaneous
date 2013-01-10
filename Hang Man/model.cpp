#include "model.h"
#include <QStringList>
#include <QFile>
#include <QTextStream>
#include <QDebug>



/** @file model.h
*   @brief model::model This is the brain of the hang man game
*
*   model handles all actions asociated with words and guesses of the word this includes loading, reseting, etc.
*
*/
model::model(QObject *parent) :
    QObject(parent)
{
    //set up a new game
    NewGame();
}

model::~model()
{
    //Free global variables
    // This crashes the program
    //delete &currentWord;
    //delete &currentguessedword;
    //delete &guessesedLetters;
    //delete &guessesLeft;
}

/**
 * @brief model::NewGame clears all previous game settings and loading of new word
 */
void model::NewGame()
{
    //Get a new word
    currentWord =  LoadWord();

    //Reset the number of guesses
    guessesLeft = 10;

    // If this is not the first game clear the previous word and guessed letters
    if (!currentguessedword.isEmpty())
    {
        currentguessedword.clear();
        guessesedLetters.clear();
    }

    //Consele the new word with stars (*) one * for each letter of the word
    for(int temp = 0; temp <= currentWord.count()-1; temp++)
    {
        currentguessedword.append("*");
    }

}

/**
 * @brief model::currentGuess checkthe players guess against the word
 *
 * Checks the guessed letter against the the currentWord and places the letters in the corect place for the currentGuessedWord to be displaied to the user
 *  minuses one from the guessesLeft if the letter is not in the word and adds the letter to the guessedLetters var.
 *
 * @param guess
 * @return
 */
QString model::currentGuess(QString guess)
{
    //Check the letter has not already been guessed and that the guess is a letter
    if(!guessesedLetters.contains(guess) && QString("abcdefghijklmnopqrstuvwxyz").contains(guess) && guess !="")
    {
        //add the new guess to the list of previous guesses
        guessesedLetters.append(guess);

        //if the guess is in the word loop through to make sure all instances are unobscured
        if (currentWord.contains(guess))
        {
            int previousIndex = -1;
            do{
                previousIndex = currentWord.indexOf(guess,previousIndex+1);
                if (previousIndex!=-1) currentguessedword.replace(previousIndex, guess);
            }while(previousIndex != -1);
        }
        // remove a guess from the guessesLeft counter
        else
        {
                guessesLeft--;
        }

        //return the new less obscured word to be displaied to the user
        return currentguessedword.join("");
    }
    else if (guessesedLetters.contains(guess) && guess !="")
    {
        // if the user has already guessed this letter do nothing
        return currentguessedword.join("");
    }
    else if (currentWord.compare(guess) && guess !="" && guess.count() == currentWord.count())
    {
        //if the guess matches the currentWord then the player has won
        return currentWord;
    }

    //if the guess is not part of the alphabet and its not the word then just forget the guess and return the currentguessedword
    return currentguessedword.join("");
}

/**
 * @brief model::LoadWord Loads words from a file
 *
 * Loads a random word from the wordlist.txt file for a new game
 *
 * @return
 */
QString model::LoadWord()
{
    // to hold the word
    QString word;

    //the file to be read from
    QFile file(workingDirectory+"/wordlist.txt");

    //checkif we can find and open the file else throw an error
   if (file.open(QIODevice::ReadOnly | QIODevice::Text))
   {
        QTextStream stream(&file);

        //get a random number for the loop
        int rand = qrand() %172000;

        //loop through the lines till we get to the random one
        for (int tmp=rand;tmp>0;tmp--)
        {
            stream.readLine();
        }

        //read the random word then close the file
        word = stream.readLine();
        file.close();
   }
   else
   {
       qDebug() << "Can't open wordList.txt :(";
       //throw;
   }

   //return the word in lower case
   return word.toLower();
}

/**
 * @brief model::GetCurrentGuessedWord
 * @return
 *
 *Getter for the current state of the word being guessed (***o* , an*ma*, etc)
 *   As this is QStringList it must be joined into a QString to be displaied
 */
QString model::GetCurrentGuessedWord()
{
    //join the array into a string
    return currentguessedword.join("");
}

/**
 * @brief model::GetWord
 * @return
 *
 * Getter for the current word being guessed by the user
 */
QString model::GetWord()
{
    return currentWord;
}
