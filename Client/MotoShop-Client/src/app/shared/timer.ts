export class Timer{
    public static async wait(miliseconds:number)
    {
        return new Promise((resolve) => setTimeout(resolve, miliseconds));
    }
}

