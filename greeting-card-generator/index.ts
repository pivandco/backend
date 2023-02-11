import express from "express";
import { query, validationResult } from "express-validator";
import {
  Gender,
  generateGreetingCardImage,
  GreetingCardInput,
} from "./src/greetingCard";
import holidays from "./src/holidays.json";

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
  query("holiday").isIn(holidays.map((h) => h.id)),

  async (req, res) => {
    const errors = validationResult(req);
    if (!errors.isEmpty()) {
      return res.status(400).json({ errors: errors.array() });
    }

    const q = req.query as GreetingCardQuery;
    const input = {
      ...q,
      holiday: holidays.find((h) => h.id === q.holiday)!,
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
