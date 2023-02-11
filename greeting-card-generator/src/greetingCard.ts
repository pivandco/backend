import fs from "fs";
import Jimp from "jimp";
import { file } from "tmp-promise";
import holidays from "./holidays.json";

export enum Gender {
  Female = "Female",
  Male = "Male",
}

export interface Holiday {
  id: string;
  name: string;
}

export const getHolidays = () => holidays;

export interface GreetingCardInput {
  firstName: string;
  middleName?: string;
  lastName?: string;
  gender: Gender;
  holiday: Holiday;
}

interface BackgroundImage {
  jimp: Jimp;
  format: "jpg" | "png";
}

async function getRandomBackgroundImageForHoliday(
  holiday: Holiday
): Promise<BackgroundImage> {
  const backgroundsDir = `cardTemplates/${holiday.id}/`;
  const backgroundImageFiles = fs.readdirSync(backgroundsDir); // TODO: make this async
  const randomIndex = Math.floor(Math.random() * backgroundImageFiles.length);
  const randomFilename = backgroundImageFiles[randomIndex];
  return {
    jimp: await Jimp.read(backgroundsDir + randomFilename),
    format: randomFilename.split(".").pop() as "jpg" | "png",
  };
}

export interface DisposableImageFile {
  path: string;
  cleanup: () => void;
}

export async function generateGreetingCardImage(
  input: GreetingCardInput
): Promise<DisposableImageFile> {
  const background = await getRandomBackgroundImageForHoliday(input.holiday);
  const font = await Jimp.loadFont(Jimp.FONT_SANS_32_WHITE);

  background.jimp.print(
    font,
    10,
    10,
    `${input.firstName} ${input.middleName} ${input.lastName}, поздравляем с ${input.holiday.name}!`
  );

  const imageFile = await file({ postfix: `.${background.format}` });
  await background.jimp.writeAsync(imageFile.path);

  return imageFile;
}
