import fs from "fs";
import gmLib from "gm";
import Jimp from "jimp";
import { file } from "tmp-promise";
import { Holiday, TemplateConfig } from "./holidays";

export enum Gender {
  Female = "Female",
  Male = "Male",
}

export interface GreetingCardInput {
  firstName: string;
  middleName?: string;
  lastName?: string;
  gender: Gender;
  holiday: Holiday;
  template?: number;
}

interface BackgroundImage {
  jimp: Jimp;
  format: "jpg" | "png";
}

async function getRandomTemplateForHoliday(
  holiday: Holiday
): Promise<[BackgroundImage, TemplateConfig?]> {
  const backgroundsDir = `cardTemplates/${holiday.id}/`;
  const backgroundImageFiles = fs
    .readdirSync(backgroundsDir)
    .filter((f) => imageFileNameRegex.test(f)); // TODO: make this async
  const randomIndex = Math.floor(Math.random() * backgroundImageFiles.length);
  return await getTemplateForHoliday(randomIndex, holiday);
}

const imageFileNameRegex = /.*\.(png|jpg)/;
async function getTemplateForHoliday(
  templateIndex: number,
  holiday: Holiday
): Promise<[BackgroundImage, TemplateConfig?]> {
  const backgroundsDir = `cardTemplates/${holiday.id}/`;
  const backgroundImageFiles = fs
    .readdirSync(backgroundsDir)
    .filter((f) => imageFileNameRegex.test(f)); // TODO: make this async
  const randomFilename = backgroundImageFiles[templateIndex];
  return [
    {
      jimp: await Jimp.read(backgroundsDir + randomFilename),
      format: randomFilename.split(".").pop() as "jpg" | "png",
    },
    holiday.templates?.[templateIndex],
  ];
}

export interface DisposableImageFile {
  path: string;
  cleanup: () => void;
}

const gm = gmLib.subClass({ imageMagick: "7+" });

const imageMagickAligns = {
  left: "NorthWest",
  center: "North",
  right: "NorthEast",
};
async function printTextToImage(
  text: string,
  templateConfig?: TemplateConfig
): Promise<DisposableImageFile> {
  const outFile = await file({ postfix: ".png" });

  const operationPromise = new Promise<DisposableImageFile>(
    (resolve, reject) => {
      gm(2000, 1000, "#00000000")
        .font("Arial.ttf", templateConfig?.fontSize ?? 26)
        .fill(templateConfig?.textColor ?? "#ffffff")
        .drawText(
          0,
          0,
          text,
          imageMagickAligns[templateConfig?.align ?? "left"]
        )
        .write(outFile.path, (err) => {
          if (err) {
            reject(err);
          } else {
            resolve(outFile);
          }
        });
    }
  );
  return await operationPromise;
}

function generateGreetingText(
  input: GreetingCardInput,
  templateConfig?: TemplateConfig
): string {
  const newline = templateConfig?.splitLine ? "\n" : "";
  return `${input.firstName} ${input.middleName} ${input.lastName}, ${newline} поздравляем с ${input.holiday.instrumentalCaseName}!`;
}

export async function generateGreetingCardImage(
  input: GreetingCardInput
): Promise<DisposableImageFile> {
  const [background, templateConfig] =
    typeof input.template === "number"
      ? await getTemplateForHoliday(input.template, input.holiday)
      : await getRandomTemplateForHoliday(input.holiday);
  const textFile = await printTextToImage(
    generateGreetingText(input, templateConfig),
    templateConfig
  ); // TODO: use withFile
  const textImage = await Jimp.read(textFile.path);

  background.jimp.blit(
    textImage,
    templateConfig?.textPosition[0] ?? 0,
    templateConfig?.textPosition[1] ?? 100
  );

  const imageFile = await file({ postfix: `.${background.format}` });
  await background.jimp.writeAsync(imageFile.path);

  return imageFile;
}
