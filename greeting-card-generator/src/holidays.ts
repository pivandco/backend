import { readFile } from "node:fs/promises";

export interface Holiday {
  id: string;
  name: string;
  instrumentalCaseName: string;
  templates?: TemplateConfig[];
}

export interface TemplateConfig {
  textPosition: [number, number];
  fontSize: number;
  textColor: string;
  splitLine?: boolean;
  align?: TextAlign;
}

type TextAlign = "left" | "center" | "right";

export async function getHolidays(): Promise<Holiday[]> {
  const holidaysJson = await readFile("./holidays.json", "utf-8");
  return JSON.parse(holidaysJson);  // TODO: validation?
}
