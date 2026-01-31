import React from "react";
import styles from "./styles.module.scss";
import Logo from "@/assets/images/cardImage.png";
import Image from "next/image";
import BookIcon from "@/assets/svg/BookIcon";
import { isImageFileExtensionValid } from "@/helpers/imageValidator";
import Button from "@mui/material/Button";
import { MuiMarkdown } from 'mui-markdown';
import { useRouter } from "next/router";
type Props = {
  id: string,
  courseName: string,
  coursePhoto: string,
  start: boolean,
  lastLessonId: string,
  lessonsLearned: number,
  description: string,
  totalLessonsCount: number
}
const CourseCard: React.FC<Props> = ({ id, courseName, start, coursePhoto, lastLessonId, lessonsLearned, description, totalLessonsCount }) => {
  const router = useRouter();
  return (
    <div className={styles.cardOverlay}>
      <div className={styles.cardContent}>
        <div className={styles.cardInfo}>
          <div className={styles.cardImage}>
            <Image
              src={isImageFileExtensionValid(coursePhoto) ? `data:image/png;base64,${coursePhoto}` : Logo}
              alt="Card Image"
              priority={true}
              width={200}
              height={200}
            />
          </div>
          <div className={styles.cardName}>
            <h3>{courseName}</h3>
          </div>
          <div className={styles.cardDescription}>
            <MuiMarkdown>{description}</MuiMarkdown>
          </div>
        </div>
        <div className={styles.cardActions}>
          <div className={styles.actionButton}>
            <Button
              href={`/courses/${id}`}
              className={styles[lessonsLearned === 0 ? "start" : "continues"]}
              onClick={e => { e.preventDefault(); totalLessonsCount !== 0 ? router.push(`/courses/${id}`) : router.push('#0'); }}
            >
              {lessonsLearned === 0 ? "Начать" : "Продолжить"}
            </Button>
          </div>
          <div className={styles.aboutClases}>
            <p>
              <BookIcon />{totalLessonsCount} урока
            </p>
          </div>
          <div className={styles.progressBar}>
            <div
              style={{ maxWidth: `${lessonsLearned === 0 ? 0 : Math.round((lessonsLearned / totalLessonsCount) * 100)}%` }}
              className={styles.progressBarInner}
            ></div>
          </div>
          <div className={styles.status}>
            <p>{lessonsLearned === 0 ? 'Не начато' : `Завершено на ${Math.round((lessonsLearned / totalLessonsCount) * 100)}%`}</p>
          </div>
        </div>
      </div>
    </div>
  );
};

export default CourseCard;
