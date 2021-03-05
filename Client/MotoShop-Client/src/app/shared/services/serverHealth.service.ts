import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { HealthReportModel } from "../models/server/health/HealthReport.model";
import { serverOverallHealthUrl } from "../server-urls";

@Injectable()
export class ServerHealthService {
    constructor(private client: HttpClient) { }

    getOverallHealth() {
        return this.client.get<HealthReportModel>(serverOverallHealthUrl);
    }
}