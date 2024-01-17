# Overview

This project provides a tool for recursively calculating the number of files, folders, and total file size within a specified directory and its subdirectories. The user can choose between parallel and sequential modes for the calculation.

# Usage
Command Line Arguments:

The program accepts the following command line arguments:

    -s: Run in sequential mode.
    -p: Run in parallel mode.
    -b: Run in parallel mode followed by sequential mode.

# Code Structure
# Program Class

    The Program class contains methods for sequential and parallel calculations.
    It includes a Main method to control the flow of the application based on command line arguments.

# Sequential Method

    The Sequential method recursively calculates file count, folder count, and total byte size in a sequential manner.

# ParallelCount Method

    The ParallelCount method recursively calculates file count, folder count, and total byte size using parallel threads.

# Main Method

    Parses command line arguments and executes the chosen mode.
    Displays the results, including the time taken for the calculation.

# Results

    The program outputs the number of folders, files, and total byte size for the specified directory.
    Execution time is displayed for both sequential and parallel modes (if applicable).

# How to Run

    Compile the code using a C# compiler (e.g., csc directory_stats_calculator.cs).
    Run the compiled executable with the appropriate command line arguments.

# Notes

    The program handles exceptions during file and directory processing, ensuring robustness.
