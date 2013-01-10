#ifndef MODEL_H
#define MODEL_H

#include <QObject>
#include <QStringList>

/**
 * @brief The model class MVC model for Hangman
 *
 *  Used in the model.cpp file
 */
class model : public QObject
{
    Q_OBJECT
public:

    /**
     * @brief model constructor
     * @param parent
     * @param dir
     */
    explicit model(QObject *parent = 0);
    ~model();

    /**
     * @brief NewGame
     *
     *  Resets all the required variables and calls loadword for a new game
     */
    void NewGame();

    /**
     * @brief currentGuess
     * @param guess
     * @return
     */
    QString currentGuess(QString guess);

    /**
     * @brief GetCurrentGuessedWord
     * @return
     */
    QString GetCurrentGuessedWord();

    /**
     * @brief GetWord
     * @return The current word
     */
    QString GetWord();

    /**
     * @brief guessesedLetters is the letters that have been guessed by the user
     */
    QStringList guessesedLetters;

    /**
     * @brief guessesLeft int of number of guesses left to the userin this game
     */
    int guessesLeft;

    /**
     * @brief workingDirectory
     */
    QString workingDirectory;

private:

    /**
     * @brief LoadWord reads words from a file
     *
     *  Reads the file wordlist.txt and pics a random word
     *
     * @return new word
     */
    QString LoadWord();

    /**
     * @brief currentWord word currently being guessed
     */
    QString currentWord;

    /**
     * @brief currentguessedword word with unguessed letters started (*) out
     */
    QStringList currentguessedword;

signals:
    
public slots:
    
};

#endif // MODEL_H
