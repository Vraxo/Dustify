﻿namespace Nodica;

public class Particle : ThemedRectangle
{
    public float Speed = 50;

    private float trueSpeed;
    private float driftX;
    private float verticalAcceleration;
    private Random random = new();

    public Particle()
    {
        Style.BorderColor = Color.Blank;
        Size = new(2);
    }

    public override void Start()
    {
        trueSpeed = random.Next((int)(15 * Speed), (int)(30 * Speed));
        driftX = (float)(random.NextDouble() * 2 - 1) * 100;
        verticalAcceleration = (float)(random.NextDouble() * 50);

        base.Start();
    }

    public override void Update()
    {
        Move();
        CheckForDestruction();
        base.Update();
    }

    private void Move()
    {
        float deltaTime = Time.DeltaTime;
        Position += new Vector2(driftX * deltaTime, -(trueSpeed + verticalAcceleration) * deltaTime);
        verticalAcceleration += deltaTime * 50;
    }

    private void CheckForDestruction()
    {
        if (GlobalPosition.Y < 0)
        {
            Destroy();
        }
    }
}