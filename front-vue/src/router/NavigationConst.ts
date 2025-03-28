export class NavigationConst {
  public static readonly nameApp = "Survey";

  // Login
  public static readonly routeLogin = "/";
  public static readonly nameLogin = "Login";
  public static readonly titleLogin = "Connection";

  // Register
  public static readonly routeRegister = "/register";
  public static readonly nameRegister = "Register";
  public static readonly titleRegister = "Registration";

  // List active questions
  public static readonly routeHome = "/home";
  public static readonly nameHome = "Home";
  public static readonly titleHome = "Home";

  // Active question details
  public static readonly routeActiveDetails = "/active/:id";
  public static readonly nameActiveDetails = "ActiveQuestionDetail";
  public static readonly titleActiveDetails = "Active Question Details";

  // List closed questions
  public static readonly routeClosed = "/closed";
  public static readonly nameClosed = "ClosedQuestions";
  public static readonly titleClosed = "Closed Questions";

  // Closed question details
  public static readonly routeClosedDetails = "/closed/:id";
  public static readonly nameClosedDetails = "ClosedQuestionDetail";
  public static readonly titleClosedDetails = "Closed Question Details";

  // Admin dashboard
  public static readonly routeAdmin = "/admin";
  public static readonly nameAdmin = "AdminDashboard";
  public static readonly titleAdmin = "Admin Dashboard";
}
