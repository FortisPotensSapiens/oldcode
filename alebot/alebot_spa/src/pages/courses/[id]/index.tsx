import React, { useEffect, useState } from "react";
import styles from "./styles.module.scss";
import { useRouter } from "next/router";
import api from "@/service/api";
import { MuiMarkdown } from 'mui-markdown';
const index = () => {
    const [data, setData] = useState({
        courseName: "",
        courseDescription: "",
        coursePhoto: "",
        courseFree: true,
        lastLessonId: "",
        lessonsLearned: 0,
        userCourseCompleted: false,
        lessonId: "",
        lessonName: "",
        lessonNumber: 0,
        lessonBody: ""
    })

    const router = useRouter()
    useEffect(() => {
        console.log(JSON.stringify(router));
        if (router.query?.id)
            api.get(`/api/v1/courses/${router.query.id}/first-notlearned-lesson`)
                .then(res => {
                    if (res.status === 200) {
                        setData(res.data)
                    }
                })
                .catch(e => {
                    api.get(`/api/v1/courses/${router.query.id}/lessons`)
                        .then(res => {
                            if (res.status === 200) {
                                api.get(`/api/v1/courses/lesson/${res?.data[0]?.lessonId}`)
                                    .then(res => {
                                        if (res.status === 200) {
                                            setData(res.data)
                                        }
                                    }).catch(e => console.log(e))
                            }
                        })
                        .catch(e => console.log(e))
                })
    }, [router])
    const nextLesson = () => {
        api.get(`/api/v1/courses/next-lesson/${data.lessonId}`)
            .then(res => {
                if (res.status === 200) {
                    setData(res.data)
                }
            })
            .catch(e => console.log(e))
    }
    const prevLesson = () => {
        api.get(`/api/v1/courses/prev-lesson/${data.lessonId}`)
            .then(res => {
                if (res.status === 200) {
                    setData(res.data)
                }
            })
            .catch(e => console.log(e))
    }
    return (
        <div className={styles.singleProduct}>
            {data.lessonBody.startsWith("https://docs.google") ? <div></div> : <div className={styles.productName}>
                <h3>{data.lessonName}</h3>
            </div>}
            <div className={styles.productDescription}>
                {data.lessonBody.startsWith("https://docs.google") ? <div>
                    <iframe src={data.lessonBody} width="100%" height="600px" />
                </div> : <MuiMarkdown>{data.lessonBody}</MuiMarkdown>}
            </div>
            <div className={styles.productActions}>
                {data.lessonBody.startsWith("https://docs.google") ? <div></div> : <button onClick={prevLesson}>Предыдущий урок</button>}
                {data.lessonBody.startsWith("https://docs.google") ? <div></div> : <button onClick={nextLesson}>Следующий урок</button>}
            </div>
        </div>
    );
};

export default index;
