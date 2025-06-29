export interface MaintenanceTask {
    id: number;
    description: string;
    scheduledDate: string;
    isCompleted: boolean;
    machineId: number;
}