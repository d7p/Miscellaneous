#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include "model.h"
#include "settings.h"

namespace Ui {

class MainWindow;
}

/**
 * @brief The MainWindow class
 *
 *handles all user interaction for the hangman game
 *
 */
class MainWindow : public QMainWindow
{
    Q_OBJECT
    
public:
    /**
     * @brief MainWindow constructor
     * @param parent
     */
    explicit MainWindow(QWidget *parent = 0);

    /**
     * MainWindow deconstructor
     */
    ~MainWindow();

private slots:
    /**
     * @brief on_actionGuess_triggered on enter pressed
     *
     *  Used to submit a guess on the press of the enter button
     */
    void on_actionGuess_triggered();

    /**
     * @brief on_newGame_clicked for the 'New Game' button
     *
     *  Sets up a new game for the user
     */
    void on_newGame_clicked();

    /**
     * @brief on_guess_2_clicked for the 'Guess' button
     *
     *  Used to submit a guess on the press of the button 'Guess'
     */
    void on_guess_2_clicked();

private:
    /**
     * @brief DisplayMessageBox
     *
     *   Displayes a maessage box to the user to inform them if they have won or lost
     *
     */
    void DisplayMessageBox();

    /**
     * @brief ImageSelection
     *
     *  Uses the number of goes a player has left to formate an apropreate file path string
     *
     * @param imageNum
     * @return file path string
     */
    QString ImageSelection(int imageNum);

    /**
     * @brief ui
     */
    Ui::MainWindow *ui;

    /**
     * @brief MVC
     */
    model MVC;

    /**
     * @brief Image
     *
     *Image to be displaied to the user
     */
    QPixmap Image;

    /**
     * @brief setings
     */
    Settings settings;
};

#endif // MAINWINDOW_H
