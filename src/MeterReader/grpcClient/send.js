const { ReadingPacket, ReadingStatus, ReadingMessage } = require("./meterservice_pb")
const { MeterReadingServiceClient } = require("./meterservice_grpc_web_pb")
const { Timestamp } = require('google-protobuf/google/protobuf/timestamp_pb')

const theLog = document.getElementById("theLog")
const theButton = document.getElementById("theButton")

function addToLog(msg) {
    const div = docuemnt.createElement("div")
    div.innerTest = msg
    theLog.appendChild(div)
}

theButton.addEventListener("click", function () {
    try {
        addToLog("starting service call")

        // Review generated meterservice_pb.js file to get the correct syntax and names for functions.
        const packet = new ReadingPacket()
        packet.setSuccessful(ReadingStatus.SUCCESS)

        const reading = new ReadingMessage()
        reading.setCustomerid(1)
        reading.setReadingvalue(1000)

        const time = new Timestamp()
        const now = Date.now()
        time.setSeconds(Math.round(now / 1000))

        reading.setReadingtime(time)

        packet.addReadings(reading)

        addToLog("Calling service")
        const client = new MeterReadingServiceClient(window.location.origin)

        client.addReading(packet, {}, function (err, response){
            if(err)
                addToLog(`Error: ${err}`)
            else
                addToLog(`Success: ${response.getMessage()}`)
        })

    } catch {
        addToLog("Exception thrown")
    }
})