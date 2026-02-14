namespace Lab.SendEmail.API.Configuration
{
    public static class UseAPIConfiguration
    {
        public static WebApplication UseAPI(this WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            return app;
        }
    }
}
