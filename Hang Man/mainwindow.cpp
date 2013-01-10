#include "mainwindow.h"
#include "ui_mainwindow.h"
#include "model.h"
#include "settings.h"
#include <QMessageBox>
#include <QDebug>

/**
 * @brief MainWindow::MainWindow
 *
 *The main window for the hangman game
 *
 *
 * @param parent
 */
MainWindow::MainWindow(QWidget *parent) :
    QMainWindow(parent),
    ui(new Ui::MainWindow)
{
    ui->setupUi(this);

    //set the working directory so we know where to look for the word list and settings file
    MVC.workingDirectory = qApp->applicationDirPath();
    settings.workingDirectory = qApp->applicationDirPath();

    // Set the image scale depending to the users screen/playing screen size
    Image.load(ImageSelection(10));
    Image.scaled(QSize(ui->ImageLable->width(),ui->ImageLable->height()),Qt::IgnoreAspectRatio, Qt::SmoothTransformation);

    // Setup a new game
    on_newGame_clicked();
}

/**
 * @brief MainWindow::~MainWindow deconstructor
 */
MainWindow::~MainWindow()
{
    //Free global variables
    // This crashes the program
    //delete &MVC;
    //delete &Image;

    delete ui;
}

/**
 * @brief MainWindow::on_actionGuess_triggered
 *
 *Handels all guess related actions i.e. asking the model if the guess is correct and updateing the playing screen
 */
void MainWindow::on_actionGuess_triggered()
{
    QString word = MVC.currentGuess(ui->EnterGuess->text());
    if(word.contains("*"))
    {
        //Sort the playing screen for the next guess
        ui->guessedword->setText(word);
        ui->PresviousGuesses->clear();
        ui->PresviousGuesses->addItems(MVC.guessesedLetters);
        ui->numbGuesses->display(MVC.guessesLeft);
        ui->EnterGuess->clear();

        // Display the correct image depending on how many guesses the player has left
        Image.load(ImageSelection(MVC.guessesLeft));
        ui->ImageLable->setPixmap(Image);

        // Check if the player has lost or still has a guess left
        if (MVC.guessesLeft == 0)
        {
            // If guessesLeft is 0 then the player has lost so display a message box telling them and
            DisplayMessageBox();
        }
    }
    else
    {
        // the player has lost so tell them
         DisplayMessageBox();
    }
}

/**
 * @brief MainWindow::on_newGame_clicked
 *
 * Sets up a new game for the player by resetting the playing screen and call NewGame from the model
 */
void MainWindow::on_newGame_clicked()
{
    MVC.NewGame();

    //reset the playing screen for a new game
    ui->guessedword->setText(MVC.GetCurrentGuessedWord());
    ui->PresviousGuesses->clear();
    ui->numbGuesses->display(MVC.guessesLeft);
    ui->EnterGuess->clear();

    //Image 10 is always the first image
    Image.load(ImageSelection(10));
    ui->ImageLable->setPixmap(Image);
}

/**
 * @brief MainWindow::ImageSelection
 * @param imageNum
 * @return  path to images
 *
 * Converts the number of goes left from an int to a file path QString to be used in the QImageViewer
 */
QString MainWindow::ImageSelection(int imageNum){
    //get the working directory of the app
    QString path = qApp->applicationDirPath();

    // append the number to the end of the directory
    switch(imageNum)
    {
    case 1:
        return path.append("/images/hangman1.jpg");
        break;
    case 2:
        return path.append("/images/hangman2.jpg");
        break;
    case 3:
        return path.append("/images/hangman3.jpg");
        break;
    case 4:
        return path.append("/images/hangman4.jpg");
        break;
    case 5:
        return path.append("/images/hangman5.jpg");
        break;
    case 6:
        return path.append("/images/hangman6.jpg");
        break;
    case 7:
        return path.append("/images/hangman7.jpg");
        break;
    case 8:
        return path.append("/images/hangman8.jpg");
        break;
    case 9:
        return path.append("/images/hangman9.jpg");
        break;
    case 10:
        return path.append("/images/hangman10.jpg");
        break;
    case 0:
        return path.append("/images/hangman0.jpg");
        break;
    }
    return "";
}

/**
 * @brief MainWindow::on_guess_2_clicked
 *
 * Handles the clicked event from the 'Guess' button
 */
void MainWindow::on_guess_2_clicked()
{
    on_actionGuess_triggered();
}

/**
 * @brief MainWindow::DisplayMessageBox displays message box to player
 *
 *  Displays a message box telling the player if they have one or lost based on the nuber of guesses they have left.
 *  The player is also offerd another game.
 */
void MainWindow::DisplayMessageBox()
{
    QMessageBox msgBox;

    // Set the message boxes text depending on if the player has won or lost
    if(MVC.guessesLeft> 0)
    {
        msgBox.setText("Congratulations You Won!!!");
        settings.Win();
    }
    else
    {
        msgBox.setText("Sorry you lost\nThe word was: "+MVC.GetWord());
        settings.Loss();
    }
    QString message = "You have\nWon: ";
         message.append(QString("%1").arg(settings.TotalGamesWon()));
         message.append("\nLost: ");
         message.append(QString("%1").arg(settings.TotalGamesLost()));
         message.append("\nTotal: ");
         message.append(QString("%1").arg(settings.TotalGamesPlayed()));
         message.append("\nWould you like to start a new game");
    msgBox.setInformativeText(message);

    // Set the buttons for a new game
    QPushButton *yes = msgBox.addButton(QMessageBox::Yes);
    msgBox.addButton(QMessageBox::No);
    msgBox.exec();

    // Handle the click even. wether the player wants to play again or not
    if(msgBox.clickedButton() == yes)
    {
        on_newGame_clicked();
    }
    else
    {
       ui->EnterGuess->clear();
       ui->guessedword->setText(MVC.GetWord());
    }
}
