﻿# Exam Bank

## Storage

การเก็บข้อมูลของ eXam Bank มีรายละเอียดที่ต้องทำนึงถึงมากมายหลายเรื่อง จึงต้องทำด้วยความระมัดระวัง เบื้องต้นได้กำหนดคุณสมบัติของแหล่งเก็บข้อมูลไว้ดังนี้
* มีการระบุ hash ของข้อมูลข้อสอบและตัวเลือกที่เป็นตัวหนังสือและรูปภาพแยกกัน เพื่อใช้ตรวจสอบการแก้ไขเมื่อมีการ upload ในภายหลัง
* แยกจัดเก็บ hash ของ resource ทุกตัว นั่นคือภาพของตัวเลือกคนละตัวเลือกแม้จะอยู่ในข้อเดียวกันจะมี hash เป็นของตัวเอง
* ในข้อเดียวกันจะนำคำถามและตัวเลือกทุกตัวมาต่อกันโดยคั่นด้วยการขึ้นบรรทัดใหม่แล้วนำมาคำนวณ hash เพื่อเปรียบเทียบกันว่ามีการแก้ไขข้อนี้หรือไม่  ที่ต้นบรรทัดของคำถามให้ขึ้นต้นด้วย NoShuffle แล้วจึงตามด้วยข้อความของคำถาม ส่วนต้นบรรทัดของตัวเลือกให้ขึ้นต้นด้วย IsCorrectAnswer แล้วค่อยตามด้วยข้อความของตัวเลือก
* การระบุว่าตัวเลือกจะถูกสลับลำดับใหม่หรือไม่ให้ระบุใน NoShuffle ของคำถาม และถ้ามีการเปลี่ยนแปลงจะทำให้ hash ของคำถามคำตอบข้อนี้เปลี่ยนแปลงไป
* เมื่อมีการเปลี่ยนแปลงเฉลยคำตอบที่ถูกต้อง จะทำให้ hash ของคำถามคำตอบข้อนี้เปลี่ยนแปลงไปด้วย
* ให้มีการแยกเก็บ hash ของถ้อยคำในคำถามคำตอบพร้อมเฉลยกับ hash เฉพาะถ้อยคำในคำถามคำตอบเท่านั้น เป็น 2 อย่าง เพราะตัวหนึ่งใช้ตรวจสอบว่ามีการแก้ไขหรือไม่ อีกตัวหนึ่งสำหรับใช้ค้นหาข้อที่มีข้อความของคำถามคำตอบเหมือนกัน(ในอนาคต)
* ข้อมูลของคำถามคำตอบและรูปภาพจะถูกจัดเก็บในตำแหน่งของตัวเองแล้วอ้างถึงจาก record หลักอย่างถูกต้องอีกที เพื่อให้การเปลี่ยน version ทำได้สะดวก
* ลำดับของรูปที่ถูกอ้างถึงใน record กับตำแหน่งที่อ้างถึงในคำถามคำตอบต้องไม่ผิดพลาดเมื่อดึงข้อมูลข้อสอบข้อนี้ขึ้นมา
* มีการจัดเก็บข้อมูลการนำข้อสอบไปใช้แยกตามหมวดหมู่ในที่เฉพาะแยกต่างหากจากตัวคำถามคำตอบ เพื่อให้ปรับเปลี่ยนหลักเกณฑ์ในภายหลังได้สะดวก
* การ grouping วิชาสอบให้ถือว่าเป็น Business Domain Specific เช่นของกรมพัฒน์วิชาสอบคือช่างเชื่อมไฟฟ้าระดับที่ 2 ถือเป็น 1 วิชาในระบบ แต่ผู้ใช้งานอาจต้องการเห็นการ grouping วิชาช่างเชื่อมไฟฟ้าทั้ง 3 ระดับ รวมเป็นเรื่องเดียวกัน กล่าวคือวิชาช่างเชื่อมไฟฟ้าเป็นเพียงการ grouping ซึ่งจะแบ่งเป็น ระดับ 1-3 และจะสอบแยกกันจึงถือเป็นคนละวิชาในระบบ
* เกณฑ์การผ่านให้ระบุเป็นรายวิชาไป
* เกณฑ์การให้คะแนนของคำถามคำตอบ จะระบุสำหรับคำถามคำตอบแต่ละข้อแยกกันโดยเฉพาะ แต่ไม่ได้เก็บรวมอยู่ในเนื้อความเพื่อให้สามารถแก้ไขภายหลังได้ง่ายขึ้น โดยทั่วไปจะเป็นการให้คะแนนข้อที่ตอบถูกเป็น 1 คะแนน ข้อที่ตอบผิดเป็น 0 คะแนน โดยในข้อเดียวกันใช้วิธีบวกในการให้คะแนน โดยให้ตอบได้ข้อละ 1 ตัวเลือก
* ตัวอย่างของเกณฑ์การให้คะแนนของคำถามคำตอบอื่น ๆ ได้แก่ ให้เลือกตอบได้มากกว่า 1 ข้อ โดยต้องตอบเฉพาะตัวเลือกที่ถูกทั้งหมดให้ครบถ้วน จะกำหนดเกณฑ์เป็นเลือกคำตอบได้มากกว่า 1 ข้อ โดยคะแนนคำตอบที่ถูกเป็นข้อละ 1 คะแนน ข้อที่ผิดเป็น 0 คะแนน ให้คะแนนโดยเอาค่าคะแนนมาคูณกัน
* อีกตัวอย่างของเกณฑ์การให้คะแนนของคำถามคำตอบอื่น ๆ ได้แก่ ให้เลือกตอบได้มากกว่า 1 ข้อ โดยต้องตอบเฉพาะตัวเลือกที่ถูก แต่ถ้าเลือกตัวเลือกที่ถูกได้มากจะได้คะแนนมากขึ้น แต่ถ้าเลือกตัวเลือกที่ผิดจะได้ 0 คะแนน เช่น ถ้ามีตัวเลือกที่ถูก 3 ตัวเลือก เลือกถูก 1 ข้อได้ 1 คะแนน เลือกถูก 2 ข้อได้ 2 คะแนน เลือกถูก 3 ข้อได้ 4 คะแนนเป็นต้น เราจะให้คะแนนคำตอบข้อที่ถูกข้อละ 2 คะแนน ข้อที่ผิดข้อละ 0 คะแนน แล้วกำหนดวิธีการให้คะแนนโดยการคูณ และกำหนด Weight ของข้อนี้เป็น 4 คะแนน
* ข้อสอบแต่ละข้อจะมี Weight ในการไปคำนวณคะแนนรวม โดยทั่วไป weight จะมีค่าเป็น 1 ยกเว้นบางข้อที่คิดว่ายากมากอาจให้ weight มากกว่านั้นก็ได้ ส่วน ratio ในการคำนวณจากคะแนนที่ตอบได้ให้กลับมาเป็น weight ของข้อสอบข้อนั้น จะให้ระบบคำนวณเพื่อเก็บไว้ใช้งาน โดยผู้ใช้จะใส่เฉพาะ weight ของคำถามเท่านั้น

### ประเด็นที่ควรทดสอบ

* JsonSerializer ที่ใช้สามารถอ่านข้อมูล json ที่มีข้อมูลมากกว่าที่ระบุใน model ได้

## การสุ่มข้อสอบ

เมื่อได้รับคำร้องขอชุดข้อสอบสำหรับทำการทดสอบ eXam Bank จะทำการสุ่มข้อสอบและเข้ารหัสคำถามคำตอบ และ เฉลยให้ตามจำนวนที่ร้องขอดังนี้
* ถ้าในคำถามคำตอบไม่ได้มีเพียงข้อความอย่างเดียว เมื่อทำการสุ่มและสลับคำตอบแล้ว ต้องได้ผลลัพธ์ที่มีการเรียงลำดับถูกต้องตรงกันไม่มีความผิดพลาดคลาดเคลื่อน
* การสุ่มคำถาม จะสุ่มตามหมวดมาให้ได้จำนวนข้อตามที่กำหนด
* หากจำนวนข้อที่กำหนดน้อยกว่าจำนวนข้อสอบที่มี ให้มีการแจ้งเตือนด้วยวิธีที่เหมาะสม แต่จะสามารถสุ่มได้สำเร็จคือได้ข้อสอบที่มีทั้งหมด
* หลังจากสุ่มคำถามแล้ว ต้องทำการสลับลำดับของคำถามด้วย

### เทคนิคที่ใช้ในการสุ่ม
- จะทำการสุ่มข้อสอบเป็นลำดับแรก
- จากนั้นจะสลับลำดับคำถามในข้อสอบที่สุ่มมาได้
- แล้วจึงค่อยทำการสลับตัวเลือกในคำถามเป็นลำดับถัดมา