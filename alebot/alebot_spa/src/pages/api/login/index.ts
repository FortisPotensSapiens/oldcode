import type { NextApiRequest, NextApiResponse } from "next";

export type TData = {
  tokenType: string;
  accessToken: string;
  expiresIn: number;
  refreshToken: string;
};
export type TError = { error: string };

export const accessToken = "your jwt can be here";
export const refreshToken = "your jwt can be here";

export default function handler(
  req: NextApiRequest,
  res: NextApiResponse<TData | TError>
) {
  try {
    if (req.method === "POST") {
      const body = req.body;

      if (body.email === "admin@admin.com" && body.password === "admin") {
        res.status(200).json({
          tokenType: "string",
          accessToken,
          expiresIn: 0,
          refreshToken,
        });
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
