# Portfolio Website - Shakhoyat Shujon

A modern, responsive portfolio website built with ASP.NET Core 9.0, featuring a professional resume section with PDF download functionality, project showcases, and comprehensive data science achievements.

## 🌟 Features

- **Professional Resume**: Interactive resume with PDF download capability
- **Project Portfolio**: Showcase of development projects with live demos
- **Skills & Achievements**: Comprehensive display of technical skills and accomplishments
- **Kaggle Integration**: Display of competitive programming and data science achievements
- **Responsive Design**: Mobile-first design with dark mode support
- **Database Integration**: Entity Framework with SQL Server for data management
- **Identity System**: ASP.NET Core Identity for user authentication
- **AI Integration**: Gemini AI service integration for enhanced functionality

## 🛠️ Tech Stack

- **Backend**: ASP.NET Core 9.0 (C#)
- **Frontend**: Razor Pages, HTML5, CSS3, JavaScript
- **Database**: SQL Server with Entity Framework Core
- **Authentication**: ASP.NET Core Identity
- **Styling**: Tailwind CSS, Custom CSS
- **PDF Generation**: jsPDF + html2canvas
- **Icons**: Font Awesome
- **External APIs**: GitHub API, Kaggle API integration

## 📋 Prerequisites

Before running this application, ensure you have the following installed:

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) or later
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (LocalDB or full version)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)
- [Git](https://git-scm.com/) for version control

## 🚀 Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/Shakhoyat/portfolio-website.git
cd portfolio-website
```

### 2. Database Setup

The application uses Entity Framework Core with SQL Server. The database will be created automatically on first run.

#### Connection String Configuration

Update the connection string in `appsettings.json` if needed:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=PortfolioWebsite;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

#### Apply Database Migrations

```bash
dotnet ef database update
```

### 3. Build the Application

```bash
dotnet build PortfolioWebsite.csproj
```

### 4. Run the Application

```bash
dotnet run --project PortfolioWebsite.csproj
```

The application will start and be available at:
- **HTTP**: http://localhost:5005
- **HTTPS**: https://localhost:7005 (if configured)

## 📁 Project Structure

```
portfolio-website/
├── Areas/                  # ASP.NET Core Areas (Identity)
├── Controllers/           # MVC Controllers
├── Data/                 # Database Context and Configurations
├── Migrations/           # Entity Framework Migrations
├── Models/              # Data Models and ViewModels
├── Services/            # Business Logic Services
├── Views/               # Razor Views and Templates
├── wwwroot/            # Static Files (CSS, JS, Images)
├── Program.cs          # Application Entry Point
└── appsettings.json   # Configuration Settings
```

## 🎯 Key Sections

### Home Page
- Hero section with professional introduction
- Featured projects and achievements
- Skills overview and contact information

### Resume Section
- **Professional formatting** with GitHub-integrated content
- **PDF Download**: Click "Download PDF" to generate a professional resume
- **Print Optimization**: Styled for high-quality printing
- **Responsive Design**: Looks great on all devices

### Projects
- Detailed project showcases with descriptions
- Technology stack information
- Live demo and GitHub repository links

### Achievements
- Kaggle competition results and rankings
- Certifications and awards
- Problem-solving statistics

### Contact
- Professional contact information
- Social media links
- Contact form functionality

## 🔧 Configuration

### Environment Variables

Create `appsettings.Development.json` for development-specific settings:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Your-Development-Connection-String"
  },
  "ApiKeys": {
    "GitHub": "your-github-token",
    "Kaggle": "your-kaggle-api-key"
  }
}
```

### External API Configuration

For enhanced functionality, configure these API keys:

1. **GitHub API**: For repository and profile information
2. **Kaggle API**: For competition and dataset statistics

## 📱 Features Guide

### Resume PDF Download

1. Navigate to the Resume section: `/Home/Resume`
2. Click the "Download PDF" button
3. Wait for generation (shows loading spinner)
4. PDF file (`Shakhoyat_Shujon_Resume.pdf`) will download automatically

### Print Resume

1. Go to Resume section
2. Click "Print Resume" button
3. Use browser's print dialog for physical copies

### Dark Mode

The website automatically respects system dark mode preferences and includes manual toggle options.

## 🔧 Development

### Adding New Features

1. **Controllers**: Add new controllers in `/Controllers`
2. **Views**: Create corresponding views in `/Views`
3. **Models**: Define data models in `/Models`
4. **Services**: Implement business logic in `/Services`

### Database Changes

1. Modify models in `/Models`
2. Add migration: `dotnet ef migrations add MigrationName`
3. Update database: `dotnet ef database update`

### Frontend Customization

- **Styles**: Main styles in `/wwwroot/css/`
- **Scripts**: JavaScript files in `/wwwroot/js/`
- **Images**: Static images in `/wwwroot/images/`

## 🚦 Troubleshooting

### Common Issues

#### Build Errors
```bash
# Clean and rebuild
dotnet clean
dotnet build
```

#### Database Connection Issues
```bash
# Reset database
dotnet ef database drop
dotnet ef database update
```

#### Port Already in Use
Update ports in `Properties/launchSettings.json`:

```json
{
  "applicationUrl": "https://localhost:7006;http://localhost:5006"
}
```

### Performance Optimization

- **Database**: Indexes are configured for optimal query performance
- **Caching**: Response caching implemented for static content
- **Minification**: CSS and JS files are optimized for production

## 📊 Browser Compatibility

- ✅ Chrome 90+
- ✅ Firefox 88+
- ✅ Safari 14+
- ✅ Edge 90+
- ✅ Mobile browsers (iOS Safari, Chrome Mobile)

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch: `git checkout -b feature/new-feature`
3. Commit changes: `git commit -am 'Add new feature'`
4. Push to branch: `git push origin feature/new-feature`
5. Submit a pull request

## 📜 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 📞 Contact

**Shakhoyat Shujon**
- 📧 Email: [your-email@domain.com]
- 🔗 LinkedIn: [linkedin.com/in/shakhoyat]
- 🐱 GitHub: [github.com/Shakhoyat]
- 🏆 Kaggle: [kaggle.com/shakhoyat]

## 🙏 Acknowledgments

- ASP.NET Core team for the excellent framework
- Tailwind CSS for the utility-first CSS framework
- Font Awesome for the icon library
- jsPDF and html2canvas for PDF generation
- GitHub API for profile integration

---

**🌟 Star this repository if you find it helpful!**

**🔗 Live Demo**: [Your deployed website URL here]

---

*Last updated: August 17, 2025*
