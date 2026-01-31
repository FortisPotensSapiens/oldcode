import type { NextApiRequest, NextApiResponse } from "next";

export type TData = {};
export type TError = { error: string };

export default function handler(
  req: NextApiRequest,
  res: NextApiResponse<TData | TError>
) {
  try {
    if (req.method === "POST") {
      const body = req.body;

      if (body.code === "111111") {
        res.status(200).json({});
      } else {
        res.status(422).json({ error: "wrong credentials" });
      }
    } else {
      res.status(404).json({ error: "not found" });
    }
  } catch (e) {
    res.status(500).json({ error: "some server error" });
  }
}
