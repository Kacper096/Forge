using Forge.WebHost;

var app = Startup.CreateHost(args);

app.MapGet("/", () => "Hello World!");

app.Run();
