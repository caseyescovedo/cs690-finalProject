namespace BusShuttle;

public class Stop {
    public string Name { get; }

    public Stop(string name) {
        this.Name = name;
    }

    public override string ToString() {
        return this.Name;
    }
}

public class Driver {
    public string Name { get; }

    public Driver(string name) {
        this.Name = name;
    }

    public override string ToString() {
        return this.Name;
    }
}

public class Loop {
    public string Name { get; }
    public List<Stop> Stops { get; }

    public Loop(string name) {
        this.Name = name;
        this.Stops = new List<Stop>();
    }

    public override string ToString() {
        return this.Name;
    }
}

public class PassengerData {
    public int Boarded { get; }
    public Stop Stop { get; }
    public Loop Loop { get; }
    public Driver Driver { get; }

    public PassengerData(int boarded, Stop stop, Loop loop, Driver driver) {
        this.Boarded = boarded;
        this.Stop = stop;
        this.Loop = loop;
        this.Driver = driver;
    }
}