Select * FROM rooms
JOIN class ON rooms.roomClassId = class.classId
JOIN size ON rooms.roomSizeId = size.sizeId