import express from "express";
import { query, validationResult } from "express-validator";
import {
  Gender,
  generateGreetingCardImage,
  GreetingCardInput,
} from "./src/greetingCard";
import { getHolidays, Holiday } from "./src/holidays";

const app = express();
const port = 3000;

type GreetingCardQuery = Omit<GreetingCardInput, "holiday"> & {
  holiday: string;
};

app.get(
  "/api/greeting-card",

  query("firstName").isString(),
  query("middleName").optional().isString(),
  query("lastName").optional().isString(),
  query("gender").isIn(Object.keys(Gender)),
  // FIXME
  // query("holiday").isIn(getHolidays().map((h) => h.id)),
  query("template").optional().isInt().toInt(),

  async (req, res) => {
    const errors = validationResult(req);
    if (!errors.isEmpty()) {
      return res.status(400).json({ errors: errors.array() });
    }

    const holidays = await getHolidays();
    const q = req.query as GreetingCardQuery;
    const input: GreetingCardInput = {
      ...q,
      holiday: holidays.find((h) => h.id === q.holiday)! as Holiday,
    };
    const greetingCardImage = await generateGreetingCardImage(input);

    res.sendFile(greetingCardImage.path, () => {
      greetingCardImage.cleanup();
    });
  }
);

app.listen(port, () => {
  console.log(`App listening on port ${port}`);
});
