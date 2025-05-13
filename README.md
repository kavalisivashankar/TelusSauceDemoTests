How to Install the Repo on Your Machine
Install a Text Editor and Prerequisites

Download and install a text editor or IDE such as Visual Studio, Visual Studio Code, or JetBrains Rider.

Install the latest .NET SDK (C# development tools).

Install Firefox for browser testing.

Clone the Repository

bash
git clone https://github.com/your-username/SauceDemoTests.git
cd SauceDemoTests
Restore Project Dependencies

bash
dotnet restore
This will install all necessary NuGet packages, including Selenium WebDriver, NUnit, and GeckoDriver for Firefox.

How to Execute Your Tests from the Command Line
Build the Project (Optional, but recommended):

bash
dotnet build
Run All Tests:

bash
dotnet test
